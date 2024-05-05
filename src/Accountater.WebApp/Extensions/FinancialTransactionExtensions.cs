using Accountater.Domain.Models;
using Accountater.WebApp.Models;

namespace Accountater.WebApp.Extensions
{
    public static class FinancialTransactionExtensions
    {
        public static FinancialTransactionViewModel ToFinancialTransactionViewModel(
            this FinancialTransactionInfo model)
        {
            return new FinancialTransactionViewModel
            {
                Id = model.Id,
                Account = model.Account,
                TransactionDate = model.TransactionDate,
                Description = model.Description,
                Amount = model.Amount,
                Tags = string.Join(", ", model.Tags)
            };
        }
    }
}
