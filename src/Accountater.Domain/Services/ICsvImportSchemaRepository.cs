using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICsvImportSchemaRepository
    {
        Task<IEnumerable<CsvImportSchemaInfo>> GetCsvImportSchemas(CancellationToken cancellationToken);
        Task<CsvImportSchema> DemandCsvImportSchema(CsvImportSchemaId id, CancellationToken cancellationToken);
        Task<CsvImportSchemaInfo> SaveImportMap(CsvImportSchema csvImportSchema, CancellationToken cancellationToken);
        Task DeleteImportMap(CsvImportSchemaId id, CancellationToken cancellationToken);
    }
}
