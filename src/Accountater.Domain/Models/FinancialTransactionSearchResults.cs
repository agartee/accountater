namespace Accountater.Domain.Models
{
    public class FinancialTransactionSearchResults : SearchResults
    {
        public required IEnumerable<FinancialTransactionInfo> FinancialTransactions { get; init; }
    }
}
