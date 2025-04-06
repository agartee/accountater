using Accountater.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly IMediator mediator;

        public AnalysisController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/analysis")]
        public async Task<IActionResult> Index([FromQuery] GetMonthlySpending request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }
    }
}
