﻿using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Helpers;
using Accountater.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class RuleController : Controller
    {
        private readonly IMediator mediator;

        public RuleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/rule")]
        public async Task<IActionResult> Index([FromQuery] SearchFinancialTransactionMetadataRules request)
        {
            var results = await mediator.Send(request);

            return View(results);
        }

        [HttpGet]
        [Route("/rule/create")]
        public IActionResult Create()
        {
            ViewBag.MetadataTypes = EnumHelper.CreateSelectList<FinancialTransactionMetadataType>();

            return View();
        }

        [HttpPost]
        [Route("/rule/create")]
        public async Task<IActionResult> Create([FromForm] CreateFinancialTransactionMetadataRule command)
        {
            var result = await mediator.Send(command);

            return Redirect($"/rule/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/rule/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] FinancialTransactionMetadataRuleId id)
        {
            ViewBag.MetadataTypes = EnumHelper.CreateSelectList<FinancialTransactionMetadataType>();

            var result = await mediator.Send(new DemandFinancialTransactionMetadataRule { Id = id });

            return View(result);
        }

        [HttpPost]
        [Route("/rule/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] FinancialTransactionMetadataRuleForm form)
        {
            var result = await mediator.Send(new UpdateFinancialTransactionMetadataRule
            {
                Id = form.Id,
                Name = form.Name,
                Expression = form.Expression,
                MetadataType = form.MetadataType,
                MetadataValue = form.MetadataValue,
            });

            return Redirect($"/rule/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/rule/{id}/run")]
        public async Task<IActionResult> Run([FromRoute] FinancialTransactionMetadataRuleId id)
        {
            await mediator.Send(new RunFinancialTransactionMetadataRule { Id = id });

            return Redirect($"/rule/{id.Value}/edit");
        }

        [HttpPost]
        [Route("/rule/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] FinancialTransactionMetadataRuleId id)
        {
            await mediator.Send(new DeleteFinancialTransactionMetadataRule { Id = id });

            return Redirect($"/rule");
        }
    }
}
