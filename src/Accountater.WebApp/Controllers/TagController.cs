using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly IMediator mediator;

        public TagController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/tag")]
        public async Task<IActionResult> Index([FromQuery] SearchTags request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/tag/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/tag/create")]
        public async Task<IActionResult> Create([FromForm] CreateTag command)
        {
            var result = await mediator.Send(command);

            return Redirect($"/tag/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/tag/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] TagId id)
        {
            var result = await mediator.Send(new DemandTag { Id = id });

            return View(result);
        }

        [HttpPost]
        [Route("/tag/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] TagForm form)
        {
            var result = await mediator.Send(new UpdateTag
            {
                Id = form.Id,
                Value = form.Value,
                Color = form.Color,
                Order = form.Order,
            });

            return Redirect($"/tag/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/tag/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] TagId id)
        {
            await mediator.Send(new DeleteTag { Id = id });

            return Redirect($"/tag");
        }
    }
}
