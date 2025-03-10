using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record CsvImportSchemaMappingData
    {
        public const string TableName = "CsvImportSchemaMapping";

        public required Guid ImportSchemaId { get; init; }
        public required string MappedProperty { get; set; }
        public int ColumnIndex { get; set; }

        public CsvImportSchemaData? ImportSchema { get; set; }
    }
}
