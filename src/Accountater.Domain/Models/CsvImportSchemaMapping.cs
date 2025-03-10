namespace Accountater.Domain.Models
{
    public record CsvImportSchemaMapping
    {
        public required string MappedProperty { get; set; }
        public required int ColumnIndex { get; set; }
    }
}
