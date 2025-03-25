using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateCsvImportSchema : IRequest<CsvImportSchemaInfo>
    {
        public required string Name { get; init; }
        public IEnumerable<CsvImportSchemaMapping> Mappings { get; init; } = [];
    }

    public class CreateCsvImportSchemaHandler : IRequestHandler<CreateCsvImportSchema, CsvImportSchemaInfo>
    {
        private readonly ICsvImportSchemaRepository csvImportSchemaRepository;

        public CreateCsvImportSchemaHandler(ICsvImportSchemaRepository csvImportSchemaRepository)
        {
            this.csvImportSchemaRepository = csvImportSchemaRepository;
        }

        public async Task<CsvImportSchemaInfo> Handle(CreateCsvImportSchema request, CancellationToken cancellationToken)
        {
            var csvImportSchema = new CsvImportSchema
            {
                Id = CsvImportSchemaId.NewId(),
                Name = request.Name,
                Mappings = [.. request.Mappings]
            };

            return await csvImportSchemaRepository.SaveCsvImportSchema(csvImportSchema, cancellationToken);
        }
    }
}
