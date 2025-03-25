using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public class UpdateCsvImportSchema : IRequest<CsvImportSchemaInfo>
    {
        public required CsvImportSchemaId Id { get; init; }
        public string? Name { get; init; }
        public List<CsvImportSchemaMapping>? Mappings { get; init; }
    }

    public class UpdateCsvImportSchemaHandler : IRequestHandler<UpdateCsvImportSchema, CsvImportSchemaInfo>
    {
        private readonly ICsvImportSchemaRepository csvImportSchemaRepository;

        public UpdateCsvImportSchemaHandler(ICsvImportSchemaRepository csvImportSchemaRepository)
        {
            this.csvImportSchemaRepository = csvImportSchemaRepository;
        }

        public async Task<CsvImportSchemaInfo> Handle(UpdateCsvImportSchema request, CancellationToken cancellationToken)
        {
            var csvImportSchema = await csvImportSchemaRepository.DemandCsvImportSchema(request.Id, cancellationToken);

            if (request.Name != null)
                csvImportSchema.Name = request.Name;

            if (request.Mappings != null)
                csvImportSchema.Mappings = request.Mappings;

            return await csvImportSchemaRepository.SaveCsvImportSchema(csvImportSchema, cancellationToken);
        }
    }
}
