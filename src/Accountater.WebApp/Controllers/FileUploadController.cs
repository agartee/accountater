using Accountater.Domain.Commands;
using Accountater.WebApp.Models;
using Accountater.WebApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly ICheckingTransactionCsvParser checkingTransactionCsvParser;
        private readonly ICreditTransactionCsvParser creditTransactionCsvParser;

        public FileUploadController(IMediator mediator,
            ICheckingTransactionCsvParser checkingTransactionCsvParser,
            ICreditTransactionCsvParser creditTransactionCsvParser)
        {
            this.mediator = mediator;
            this.checkingTransactionCsvParser = checkingTransactionCsvParser;
            this.creditTransactionCsvParser = creditTransactionCsvParser;
        }

        [HttpGet]
        [Route("/fileupload")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/fileupload")]
        public async Task<IActionResult> UploadFile(UploadCsvFileViewModel model)
        {
            using var stream = model.File.OpenReadStream();
            using var reader = new StreamReader(stream);

            IRequest? request = null;

            switch (model.CsvFileType)
            {
                case CsvFileType.Checking:
                    request = new ImportCheckingTransactions { Transactions = checkingTransactionCsvParser.Parse(stream) };
                    break;
                case CsvFileType.Credit:
                    request = new ImportCreditTransactions { Transactions = creditTransactionCsvParser.Parse(stream) };
                    break;
                default:
                    throw new InvalidOperationException("Unsupported CSV File Type");
            }

            // TODO: process models
            await mediator.Send(request);

            return Redirect("/fileupload");
        }
    }
}
