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

        public FinancialTransaction ToFinancialTransaction()
        {
            var financialTransaction = new FinancialTransaction
            {
                Id = Id,
                AccountId = Account.Id,
                Date = Date,
                Description = Description,
                Amount = Amount
            };

            foreach (var tag in Tags)
                financialTransaction.AddTag(tag);

            return financialTransaction;
        }
    }
}
