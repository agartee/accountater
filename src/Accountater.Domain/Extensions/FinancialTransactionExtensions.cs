using Accountater.Domain.Models;

namespace Accountater.Domain.Extensions
{
    public static class FinancialTransactionExtensions
    {
        public static FinancialTransaction ToFinancialTransaction(this FinancialTransactionInfo financialTransactionInfo)
        {
            return new FinancialTransaction
            {
                Id = financialTransactionInfo.Id,
                AccountId = financialTransactionInfo.Account.Id,
                Date = financialTransactionInfo.Date,
                Description = financialTransactionInfo.Description,
                Amount = financialTransactionInfo.Amount,
                Tags = financialTransactionInfo.Tags.ToList()
            };
        }
    }
}
