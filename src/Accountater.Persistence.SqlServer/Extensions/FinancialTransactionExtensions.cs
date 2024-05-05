using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;
using System.Collections.Immutable;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class FinancialTransactionExtensions
    {
        public static FinancialTransactionInfo ToFinancialTransactionInfo(this FinancialTransactionData model)
        {
            return new FinancialTransactionInfo
            {
                Id = new FinancialTransactionId(model.Id),
                AccountId = new AccountId(model.AccountId),
                TransactionDate = model.TransactionDate,
                Description = model.Description,
                Amount = model.Amount,
                Tags = model.Tags.Select(t => t.Value).ToImmutableList(),
            };
        }
    }
}
