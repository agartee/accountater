using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
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
        [Route("/financialTransaction")]
        public async Task<IActionResult> Index([FromQuery] SearchFinancialTransactions request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/financialTransaction/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] FinancialTransactionId id)
        {
            var result = await mediator.Send(new DemandFinancialTransaction { Id = id });

            return View(result);
        }

        [HttpPost]
        [Route("/financialTransaction/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] FinancialTransactionForm form)
        {
            await mediator.Send(new UpdateFinancialTransaction
            {
                Id = form.Id,
                Tags = !string.IsNullOrWhiteSpace(form.Tags)
                    ? form.Tags!.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim())
                    : Enumerable.Empty<string>()
            });

            return Redirect($"/financialTransaction/{form.Id.Value}/edit");
        }
    }
}
