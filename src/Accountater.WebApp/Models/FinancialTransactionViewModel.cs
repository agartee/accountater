using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public class FinancialTransactionViewModel
    {
        public required FinancialTransactionId Id { get; init; }
        public required AccountInfo Account { get; init; }
        public required DateTime TransactionDate { get; init; }
        public required string Description { get; init; }
        public required decimal Amount { get; init; }
        public string? Tags { get; init; }
    }
}
