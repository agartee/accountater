using Accountater.WebApp.Attributes;

namespace Accountater.WebApp.Models
{
    public record UploadCsvFileViewModel
    {
        [ValidCsvFile]
        public required IFormFile File { get; init; }
        public required CsvFileType CsvFileType { get; init; }
    }
}
