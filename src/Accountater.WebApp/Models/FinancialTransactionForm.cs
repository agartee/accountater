using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public record FinancialTransactionForm
    {
        public required FinancialTransactionId Id { get; set; }
        public CategoryId? CategoryId { get; set; }
        public string? Tags { get; set; }
    }
}
