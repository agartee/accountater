using Accountater.Domain.Commands;
using Accountater.Domain.Queries;
using Accountater.Domain.Services;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IFinancialTransactionCsvParser financialTransactionCsvParser;

        public FileUploadController(IMediator mediator,
            IFinancialTransactionCsvParser financialTransactionCsvParser)
        {
            this.mediator = mediator;
            this.financialTransactionCsvParser = financialTransactionCsvParser;
        }

        [HttpGet]
        [Route("/fileupload")]
        public async Task<IActionResult> Index()
        {
            var accounts = await mediator.Send(new ListAllAccounts());
            var csvImportSchemas = await mediator.Send(new ListAllCsvImportSchemas());

            ViewBag.Accounts = accounts;
            ViewBag.CsvImportSchemas = csvImportSchemas;

            return View();
        }

        [HttpPost]
        [Route("/fileupload")]
        public async Task<IActionResult> UploadFile(UploadCsvFileViewModel model)
        {
            using var csvFileStream = model.File!.OpenReadStream();
            await mediator.Send(new ImportFinancialTransactions
            {
                CsvImportSchemaId = model.CsvImportSchemaId,
                AccountId = model.AccountId,
                CsvFileStream = csvFileStream
            });

            return Redirect("/fileupload");
        }
    }
}
