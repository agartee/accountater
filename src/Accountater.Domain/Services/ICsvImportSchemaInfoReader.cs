using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICsvImportSchemaInfoReader
    {
        Task<CsvImportSchemaInfo> DemandCsvImportSchemaInfo(CsvImportSchemaId id, CancellationToken cancellationToken);
        Task<IEnumerable<CsvImportSchemaInfo>> ListCsvImportSchemas(CancellationToken cancellationToken);
    }
}
