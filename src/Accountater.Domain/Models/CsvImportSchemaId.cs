namespace Accountater.Domain.Models
{
    public readonly record struct CsvImportSchemaId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static CsvImportSchemaId NewId()
        {
            return new CsvImportSchemaId(Guid.NewGuid());
        }
    }
}
