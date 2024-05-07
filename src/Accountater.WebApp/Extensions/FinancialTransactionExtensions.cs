using Accountater.Domain.Models;
using Accountater.WebApp.Models;

namespace Accountater.WebApp.Extensions
{
    public static class FinancialTransactionExtensions
    {
        public static EditFinancialTransactionViewModel ToFinancialTransactionViewModel(
            this FinancialTransactionInfo model)
        {
            return new EditFinancialTransactionViewModel
            {
                Id = model.Id,
                Account = model.Account,
                TransactionDate = model.Date,
                Description = model.Description,
                Amount = model.Amount,
                Tags = string.Join(", ", model.Tags)
            };
        }
    }
}
