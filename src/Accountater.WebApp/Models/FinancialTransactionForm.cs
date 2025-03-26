using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public class FinancialTransactionForm
    {
        public required FinancialTransactionId Id { get; init; }
        public string? Tags { get; init; }
    }
}
