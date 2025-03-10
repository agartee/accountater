namespace Accountater.Domain.Models
{
    public record CsvImportSchemaInfo
    {
        public required CsvImportSchemaId Id { get; init; }
        public required string Name { get; init; }
        public IEnumerable<CsvImportSchemaMappingInfo> Mappings { get; init; } = [];
    }
}
