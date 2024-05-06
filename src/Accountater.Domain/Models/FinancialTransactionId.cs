namespace Accountater.Domain.Models
{
    public record FinancialTransactionId : Id<Guid>
    {
        public FinancialTransactionId(Guid value) : base(value)
        {
        }

        public static FinancialTransactionId NewId()
        {
            return new FinancialTransactionId(Guid.NewGuid());
        }
    }
}
