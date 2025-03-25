namespace Accountater.Domain.Models
{
    public record CsvImportSchema
    {
        public required CsvImportSchemaId Id { get; init; }
        public required string Name { get; set; }
        public List<CsvImportSchemaMapping> Mappings { get; set; } = [];
    }
}
