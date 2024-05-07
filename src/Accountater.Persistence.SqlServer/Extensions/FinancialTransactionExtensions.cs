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
                Account = new AccountInfo
                {
                    Id = new AccountId(model.AccountId),
                    Name = model.Account!.Name,
                    Description = model.Account!.Description
                },
                Date = model.Date,
                Description = model.Description,
                Amount = model.Amount,
                Tags = model.Tags
                    .OrderBy(t => t.Value)
                    .Select(t => t.Value)
                    .ToImmutableList(),
            };
        }
    }
}
