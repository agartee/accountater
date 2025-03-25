namespace Accountater.Domain.Models
{
    public record CsvImportSchemaMapping
    {
        public required string MappedProperty { get; init; }
        public required int ColumnIndex { get; init; }
    }
}
