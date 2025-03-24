namespace Accountater.Domain.Models
{
    public record CsvImportSchemaId : Id<Guid>
    {
        public CsvImportSchemaId(Guid value) : base(value)
        {
        }

        public static CsvImportSchemaId NewId()
        {
            return new CsvImportSchemaId(Guid.NewGuid());
        }
    }
}
