namespace Accountater.Domain.Models
{
    public record CsvImportSchemaId : Id<Guid>
    {
        public CsvImportSchemaId(Guid value) : base(value)
        {
        }

        public static AccountId NewId()
        {
            return new AccountId(Guid.NewGuid());
        }
    }
}
