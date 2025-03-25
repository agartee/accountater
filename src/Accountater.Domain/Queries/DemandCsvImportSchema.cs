using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace CsvImportSchemaater.Domain.Queries
{
    public class DemandCsvImportSchema : IRequest<CsvImportSchemaInfo>
    {
        public required CsvImportSchemaId Id { get; init; }
    }

    public class DemandCsvImportSchemaHandler : IRequestHandler<DemandCsvImportSchema, CsvImportSchemaInfo>
    {
        private readonly ICsvImportSchemaInfoReader csvImportSchemaInfoReader;

        public DemandCsvImportSchemaHandler(ICsvImportSchemaInfoReader csvImportSchemaInfoReader)
        {
            this.csvImportSchemaInfoReader = csvImportSchemaInfoReader;
        }

        public async Task<CsvImportSchemaInfo> Handle(DemandCsvImportSchema request, CancellationToken cancellationToken)
        {
            return await csvImportSchemaInfoReader.DemandCsvImportSchemaInfo(request.Id, cancellationToken);
        }
    }
}
