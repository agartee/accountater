using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public record CsvImportSchemaForm
    {
        public required CsvImportSchemaId Id { get; init; }
        public required string Name { get; set; }
        public List<CsvImportSchemaMapping> Mappings { get; set; } = [];
    }
}
