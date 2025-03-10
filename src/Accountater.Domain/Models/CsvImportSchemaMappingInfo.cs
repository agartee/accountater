namespace Accountater.Domain.Models
{
    public record CsvImportSchemaMappingInfo
    {
        public required string MappedProperty { get; init; }
        public required int ColumnIndex { get; init; }
    }
}
