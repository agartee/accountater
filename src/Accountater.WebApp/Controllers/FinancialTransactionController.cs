using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Extensions;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class FinancialTransactionController : Controller
    {
        private readonly IMediator mediator;

        public FinancialTransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/transaction")]
        public async Task<IActionResult> Index([FromQuery] SearchFinancialTransactions request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/transaction/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] FinancialTransactionId id)
        {
            var results = await mediator.Send(new DemandFinancialTransaction { Id = id });

            return View(results.ToFinancialTransactionViewModel());
        }

        [HttpPost]
        [Route("/transaction/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] EditFinancialTransactionViewModel viewModel)
        {
            await mediator.Send(new UpdateFinancialTransaction
            {
                Id = viewModel.Id,
                Tags = !string.IsNullOrWhiteSpace(viewModel.Tags)
                    ? viewModel.Tags!.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim())
                    : Enumerable.Empty<string>()
            });

            return Redirect($"/transaction/{viewModel.Id.Value}/edit");
        }
    }
}
