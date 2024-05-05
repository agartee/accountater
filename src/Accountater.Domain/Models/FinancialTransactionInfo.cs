namespace Accountater.Domain.Models
{

    public class FinancialTransactionInfo
    {
        public required FinancialTransactionId Id { get; init; }
        public required AccountId AccountId { get; init; }
        public required DateTime TransactionDate { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public IEnumerable<string> Tags { get; init; } = new List<string>().AsReadOnly();
    }
}
