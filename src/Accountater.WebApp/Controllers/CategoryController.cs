using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/category")]
        public async Task<IActionResult> Index([FromQuery] SearchCategorys request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/category/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/category/create")]
        public async Task<IActionResult> Create([FromForm] CreateCategory command)
        {
            var result = await mediator.Send(command);

            return Redirect($"/category/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/category/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] CategoryId id)
        {
            var result = await mediator.Send(new DemandCategory { Id = id });

            return View(result);
        }

        [HttpPost]
        [Route("/category/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] CategoryForm form)
        {
            var result = await mediator.Send(new UpdateCategory
            {
                Id = form.Id,
                Name = form.Name
            });

            return Redirect($"/category/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/category/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] CategoryId id)
        {
            await mediator.Send(new DeleteCategory { Id = id });

            return Redirect($"/category");
        }
    }
}
