namespace Accountater.Domain.Models
{
    public class CsvImportSchemaSearchResults : SearchResults
    {
        public required IEnumerable<CsvImportSchema> CsvImportSchemas { get; init; }
    }
}
