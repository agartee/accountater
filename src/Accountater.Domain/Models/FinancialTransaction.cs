namespace Accountater.Domain.Models
{
    public record FinancialTransaction
    {
        private readonly List<string> tags = new();

        public required FinancialTransactionId Id { get; init; }
        public required AccountId AccountId { get; init; }
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public IEnumerable<string> Tags => tags.AsReadOnly();

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            tags.RemoveAll(t => t == tag);
        }
    }
}
