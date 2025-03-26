using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
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
            return View();
        }

        [HttpPost]
        [Route("/tagrule/create")]
        public async Task<IActionResult> Create([FromForm] CreateTagRule command)
        {
            var result = await mediator.Send(command);

            return Redirect($"/tagrule/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/tagrule/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] TagRuleId id)
        {
            var result = await mediator.Send(new DemandTagRule { Id = id });

            return View(result);
        }

        [HttpPost]
        [Route("/tagrule/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] TagRuleForm form)
        {
            var result = await mediator.Send(new UpdateTagRule
            {
                Id = form.Id,
                Name = form.Name,
                Expression = form.Expression,
                Tag = form.Tag,
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
