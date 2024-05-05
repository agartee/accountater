namespace Accountater.Domain.Models
{
    public class FinancialTransactionSearchCriteria
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; }
        public int PageIndex { get; init; }
    }
}
