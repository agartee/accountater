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
                    Type = model.Account!.Type,
                    Description = model.Account!.Description
                },
                Date = model.Date,
                Description = model.Description,
                Amount = model.Amount,
                Category = model.Category?.ToCategoryInfo(),
                Tags = model.Tags
                    .OrderBy(t => t.Value)
                    .Select(t => t.Value)
                    .ToImmutableList(),
            };
        }

        public static FinancialTransaction ToFinancialTransaction(this FinancialTransactionData model)
        {
            return new FinancialTransaction
            {
                Id = new FinancialTransactionId(model.Id),
                AccountId = new AccountId(model.AccountId),
                Date = model.Date,
                Description = model.Description,
                Amount = model.Amount,
                CategoryId = model.CategoryId != null 
                    ? new CategoryId(model.CategoryId.Value) 
                    : null,
                Tags = model.Tags
                    .OrderBy(t => t.Value)
                    .Select(t => t.Value)
                    .ToList(),
            };
        }
    }
}
