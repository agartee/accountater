namespace Accountater.Domain.Models
{
    public readonly record struct FinancialTransactionMetadataRuleId(Guid Value)
    {
        public override string ToString() => Value.ToString();


        public static FinancialTransactionMetadataRuleId NewId()
        {
            return new FinancialTransactionMetadataRuleId(Guid.NewGuid());
        }
    }
}
