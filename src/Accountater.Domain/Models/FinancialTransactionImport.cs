namespace Accountater.Domain.Models
{
    public record FinancialTransactionImport
    {
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public IEnumerable<string> Tags { get; init; } = new List<string>().AsReadOnly();

        public FinancialTransactionInfo ToFinancialTransactionInfo(FinancialTransactionId id, AccountInfo account)
        {
            return new FinancialTransactionInfo
            {
                Id = id,
                Account = account,
                Date = Date,
                Description = Description,
                Amount = Amount,
                Tags = Tags
            };
        }
    }
}
