namespace Accountater.Domain.Models
{
    public record CheckingTransaction
    {
        public DateTime Date { get; init; }
        public required string Description { get; init; }
        public required string Category { get; init; }
        public required decimal Amount { get; init; }
    }
}
