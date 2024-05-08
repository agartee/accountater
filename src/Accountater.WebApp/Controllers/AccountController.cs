using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Extensions;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/account")]
        public async Task<IActionResult> Index([FromQuery] SearchAccounts request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/account/create")]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        [Route("/account/create")]
        public async Task<IActionResult> Create([FromForm] CreateAccountViewModel viewModel)
        {
            var result = await mediator.Send(new CreateAccount
            {
                Id = AccountId.NewId(),
                Name = viewModel.Name,
                Description = viewModel.Description
            });

            return Redirect($"/account/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/account/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] AccountId id)
        {
            var results = await mediator.Send(new DemandAccount { Id = id });

            return View(results.ToAccountViewModel());
        }

        [HttpPost]
        [Route("/account/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] EditAccountViewModel viewModel)
        {
            var result = await mediator.Send(new UpdateAccount
            {
                Id = viewModel.Id ?? AccountId.NewId(),
                Name = viewModel.Name,
                Description = viewModel.Description
            });

            return Redirect($"/account/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/account/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] AccountId id)
        {
            await mediator.Send(new DeleteAccount { Id = id });

            return Redirect($"/account");
        }
    }
}
