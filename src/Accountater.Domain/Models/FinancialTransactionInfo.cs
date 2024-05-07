namespace Accountater.Domain.Models
{

    public record FinancialTransactionInfo
    {
        public required FinancialTransactionId Id { get; init; }
        public required AccountInfo Account { get; init; }
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public IEnumerable<string> Tags { get; init; } = new List<string>().AsReadOnly();
    }
}
