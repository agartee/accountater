namespace Accountater.Domain.Models
{
    public record FinancialTransactionImport
    {
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }

        public FinancialTransaction ToFinancialTransaction(FinancialTransactionId id, AccountInfo account)
        {
            return new FinancialTransaction
            {
                Id = id,
                AccountId = account.Id,
                Date = Date,
                Description = Description,
                Amount = Amount
            };
        }
    }
}
