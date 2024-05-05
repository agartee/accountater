using Accountater.Domain.Commands;
using Accountater.WebApp.Models;
using Accountater.WebApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICheckingTransactionCsvParser checkingTransactionCsvParser;
        private readonly ICreditTransactionCsvParser creditTransactionCsvParser;

        public HomeController(ICheckingTransactionCsvParser checkingTransactionCsvParser,
            ICreditTransactionCsvParser creditTransactionCsvParser)
        {
            this.checkingTransactionCsvParser = checkingTransactionCsvParser;
            this.creditTransactionCsvParser = creditTransactionCsvParser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadCsvFileViewModel model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ViewBag.Message = "Please select a valid CSV file.";
                return View("Index");
            }

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


            return View("Index");
        }
    }
}
