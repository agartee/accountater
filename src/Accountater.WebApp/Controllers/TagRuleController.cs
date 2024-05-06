using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Extensions;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class TagRuleController : Controller
    {
        private readonly IMediator mediator;

        public TagRuleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/tagrule")]
        public async Task<IActionResult> Index([FromQuery] SearchTagRules request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/tagrule/{id}")]
        public async Task<IActionResult> Edit([FromRoute] TagRuleId id)
        {
            var results = await mediator.Send(new DemandTagRule { Id = id });

            return View(results.ToTagRuleViewModel());
        }

        [HttpPost]
        [Route("/tagrule/{id}")]
        public async Task<IActionResult> Edit([FromForm] TagRuleViewModel viewModel)
        {
            await mediator.Send(new SaveTagRule
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Expression = viewModel.Expression,
                Tag = viewModel.Tag,
            });

            return Redirect($"/tagrule/{viewModel.Id.Value}");
        }
    }
}
