using Accountater.Domain.Models;
using Accountater.WebApp.Attributes;

namespace Accountater.WebApp.Models
{
    public record UploadCsvFileViewModel
    {
        public required CsvImportSchemaId CsvImportSchemaId { get; init; }
        public required AccountId AccountId { get; init; }

        [ValidCsvFile]
        public IFormFile? File { get; init; }
    }
}
