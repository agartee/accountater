namespace Accountater.Domain.Models
{
    public readonly record struct FinancialTransactionId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static FinancialTransactionId NewId()
        {
            return new FinancialTransactionId(Guid.NewGuid());
        }
    }
}
