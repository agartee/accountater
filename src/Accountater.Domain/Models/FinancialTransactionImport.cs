namespace Accountater.Domain.Models
{
    public record FinancialTransactionImport
    {
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }

        public FinancialTransaction ToFinancialTransaction(FinancialTransactionId id, AccountId accountId)
        {
            return new FinancialTransaction
            {
                Id = id,
                AccountId = accountId,
                Date = Date,
                Description = Description,
                Amount = Amount
            };
        }

        public FinancialTransactionInfo ToFinancialTransactionInfo(FinancialTransactionId id, AccountInfo account)
        {
            return new FinancialTransactionInfo
            {
                Id = id,
                Account = account,
                Date = Date,
                Description = Description,
                Amount = Amount
            };
        }
    }
}
