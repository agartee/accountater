namespace Accountater.Domain.Models
{
    public record FinancialTransactionMetadataRuleInfo
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
        public required string Name { get; init; }
        public required string Expression { get; init; }
        public required string Tag { get; init; }
    }
}
