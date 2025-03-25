using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICsvImportSchemaRepository
    {
        Task<CsvImportSchema> DemandCsvImportSchema(CsvImportSchemaId id, CancellationToken cancellationToken);
        Task<CsvImportSchemaInfo> SaveCsvImportSchema(CsvImportSchema csvImportSchema, CancellationToken cancellationToken);
        Task DeleteCsvImportSchema(CsvImportSchemaId id, CancellationToken cancellationToken);
    }
}
