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
            var accounts = await mediator.Send(new GetAllAccounts());
            ViewBag.Accounts = accounts;

            return View();
        }

        [HttpPost]
        [Route("/fileupload")]
        public async Task<IActionResult> UploadFile(UploadCsvFileViewModel model)
        {
            using var csvFileStream = model.File!.OpenReadStream();
            //using var reader = new StreamReader(csvFileStream);

            // TODO: process models
            await mediator.Send(new ImportFinancialTransactions
            {
                AccountId = model.SelectedAccountId,
                CsvFileStream = csvFileStream
            });

            return Redirect("/fileupload");
        }
    }
}
