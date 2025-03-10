using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record CsvImportSchemaData
    {
        public const string TableName = "CsvImportSchema";
        public required Guid Id { get; set; }
        public required string Name { get; set; }

        public List<CsvImportSchemaMappingData> Mappings { get; set; } = new();
    }
}
