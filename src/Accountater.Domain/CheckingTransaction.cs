namespace Accountater.Domain
{
    public record CheckingTransaction
    {
        public required DateTime TransactionDate { get; init; }
        public required string Description { get; init; }
        public required string Category { get; init; }
        public required decimal Amount { get; init; }
    }
}
