using Accountater.Domain.Commands;
using Accountater.Domain.Models;
using Accountater.Domain.Queries;
using Accountater.WebApp.Models;
using CsvImportSchemaater.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class CsvImportSchemaController : Controller
    {
        private readonly IMediator mediator;

        public CsvImportSchemaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/csvimportschema")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new ListAllCsvImportSchemas(), cancellationToken);

            return View(results);
        }

        [HttpGet]
        [Route("/csvimportschema/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/csvimportschema/create")]
        public async Task<IActionResult> Create([FromForm] CreateCsvImportSchema command)
        {
            var result = await mediator.Send(command);

            return Redirect($"/csvimportschema/{result.Id.Value}/edit");
        }

        [HttpGet]
        [Route("/csvimportschema/{id}/edit")]
        public async Task<IActionResult> Edit([FromRoute] CsvImportSchemaId id)
        {
            var result = await mediator.Send(new DemandCsvImportSchema { Id = id });
            
            return View(result);
        }

        [HttpPost]
        [Route("/csvimportschema/{id}/edit")]
        public async Task<IActionResult> Edit([FromForm] CsvImportSchemaForm form)
        {
            var result = await mediator.Send(new UpdateCsvImportSchema
            {
                Id = form.Id ?? CsvImportSchemaId.NewId(),
                Name = form.Name,
                Mappings = form.Mappings
            });

            return Redirect($"/csvimportschema/{result.Id.Value}/edit");
        }

        [HttpPost]
        [Route("/csvimportschema/{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] CsvImportSchemaId id)
        {
            await mediator.Send(new DeleteCsvImportSchema { Id = id });

            return Redirect($"/csvimportschema");
        }
    }
}
