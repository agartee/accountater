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
        [Route("/tagrule/create")]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        [Route("/tagrule/create")]
        public async Task<IActionResult> Create([FromForm] CreateTagRuleViewModel viewModel)
        {
            var result = await mediator.Send(new CreateTagRule
            {
                Id = TagRuleId.NewId(),
                Name = viewModel.Name,
                Expression = viewModel.Expression,
                Tag = viewModel.Tag,
            });

            return Redirect($"/tagrule/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/tagrule/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] TagRuleId id)
        {
            var results = await mediator.Send(new DemandTagRule { Id = id });

            return View(results.ToTagRuleViewModel());
        }

        [HttpPost]
        [Route("/tagrule/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] EditTagRuleViewModel viewModel)
        {
            var result = await mediator.Send(new UpdateTagRule
            {
                Id = viewModel.Id ?? TagRuleId.NewId(),
                Name = viewModel.Name,
                Expression = viewModel.Expression,
                Tag = viewModel.Tag,
            });

            return Redirect($"/tagrule/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/tagrule/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] TagRuleId id)
        {
            await mediator.Send(new DeleteTagRule { Id = id });

            return Redirect($"/tagrule");
        }
    }
}
