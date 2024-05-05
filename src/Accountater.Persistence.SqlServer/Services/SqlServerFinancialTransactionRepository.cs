using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerFinancialTransactionRepository : IFinancialTransactionRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerFinancialTransactionRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertCreditTransactions(IEnumerable<CreditTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var account = await GetOrAddAccount(transaction.AccountName);

                account.Transactions.Add(new FinancialTransactionData
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    TransactionDate = transaction.Date,
                    Description = transaction.Merchant,
                    Amount = transaction.Amount,
                });
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task InsertCheckingTransactions(IEnumerable<CheckingTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var account = await GetOrAddAccount("Checking");

                account.Transactions.Add(new FinancialTransactionData
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    TransactionDate = transaction.Date,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                });
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<FinancialTransactionInfo> FinancialTransactions, int TotalCount)> GetFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<FinancialTransactionData, bool>> predicate = v => v.Tags
                .Any(t => t.Value.Contains(criteria.SearchText ?? ""));

            var totalResults = await dbContext.FinancialTransactions
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var financialTransactions = await dbContext.FinancialTransactions
                .AsNoTracking()
                .Include(v => v.Tags)
                .Where(predicate)
                .OrderByDescending(v => v.TransactionDate)
                .Skip((criteria.PageIndex - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(v => v.ToFinancialTransactionInfo())
                .ToListAsync(cancellationToken);

            return (financialTransactions, totalResults);
        }

        public async Task UpdateFinancialTransactionTags(FinancialTransactionId id, IEnumerable<string> tags,
            CancellationToken cancellationToken)
        {
            var data = await dbContext.FinancialTransactions
                .Include(v => v.Tags)
                .SingleAsync(v => v.Id == id.Value, cancellationToken);

            await UpdateTags(data, tags);
        }

        private async Task<AccountData> GetOrAddAccount(string accountName)
        {
            var localAccount = dbContext.Accounts.Local
                .Where(a => a.Name == accountName)
                .SingleOrDefault();

            var account = localAccount
                ?? await dbContext.Accounts
                    .Where(a => a.Name == accountName)
                    .SingleOrDefaultAsync();

            if (account == null)
            {
                account = new AccountData
                {
                    Id = Guid.NewGuid(),
                    Name = accountName,
                };

                dbContext.Accounts.Add(account);
            }

            return account;
        }

        private async Task UpdateTags(FinancialTransactionData transactionData, IEnumerable<string> tags)
        {
            var existingTags = await dbContext.Tags
                .Where(t => tags.Contains(t.Value))
                .ToListAsync();

            foreach (var tagValue in tags)
            {
                if (transactionData.Tags.Any(t => t.Value == tagValue))
                    continue;

                var tagData = existingTags.FirstOrDefault(t => t.Value == tagValue);
                if (tagData == null)
                {
                    tagData = new TagData
                    {
                        Id = Guid.NewGuid(),
                        Value = tagValue.Truncate(200)
                    };

                    dbContext.Tags.Add(tagData);
                }
                transactionData.Tags.Add(tagData);
            }

            var tagsToRemove = transactionData.Tags
                .Where(existingTag => !tags.Contains(existingTag.Value))
                .ToList();

            foreach (var tagToRemove in tagsToRemove)
                transactionData.Tags.Remove(tagToRemove);
        }
    }
}
