namespace Accountater.Domain.Models
{
    public class FinancialTransactionSearchResults
    {
        public required IEnumerable<FinancialTransactionInfo> FinancialTransactions { get; init; }
        public string? SearchText { get; init; }
        public required int PageSize { get; init; }
        public required int PageIndex { get; init; }
        public required int TotalCount { get; init; }
    }
}
