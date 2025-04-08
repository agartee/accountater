namespace Accountater.Domain.Models
{
    public record FinancialTransaction
    {
        public required FinancialTransactionId Id { get; init; }
        public required AccountId AccountId { get; init; }
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public CategoryId? CategoryId { get; set; }
        public List<string> Tags { get; init; } = new();
    }
}
