using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record ListAllCsvImportSchemas : IRequest<IEnumerable<CsvImportSchemaInfo>>
    {
    }

    public class ListAllCsvImportSchemasHandler : IRequestHandler<ListAllCsvImportSchemas, IEnumerable<CsvImportSchemaInfo>>
    {
        private readonly ICsvImportSchemaInfoReader csvImportSchemaInfoReader;
        public ListAllCsvImportSchemasHandler(ICsvImportSchemaInfoReader csvImportSchemaInfoReader)
        {
            this.csvImportSchemaInfoReader = csvImportSchemaInfoReader;
        }
        public async Task<IEnumerable<CsvImportSchemaInfo>> Handle(ListAllCsvImportSchemas request, CancellationToken cancellationToken)
        {
            return await csvImportSchemaInfoReader.ListCsvImportSchemas(cancellationToken);
        }
    }
}
