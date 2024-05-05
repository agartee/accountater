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

        public async Task InsertCreditTransactions(IEnumerable<CreditTransaction> transactions,
            CancellationToken cancellationToken)
        {
            foreach (var transaction in transactions)
            {
                var account = await GetOrAddAccount(transaction.AccountName, cancellationToken);

                account.Transactions.Add(new FinancialTransactionData
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    TransactionDate = transaction.Date,
                    Description = transaction.Merchant,
                    Amount = transaction.Amount,
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task InsertCheckingTransactions(IEnumerable<CheckingTransaction> transactions,
            CancellationToken cancellationToken)
        {
            foreach (var transaction in transactions)
            {
                var account = await GetOrAddAccount("Checking", cancellationToken);

                account.Transactions.Add(new FinancialTransactionData
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    TransactionDate = transaction.Date,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<FinancialTransactionSearchResults> GetFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<FinancialTransactionData, bool>> predicate = v => criteria.SearchText == null
                || v.Tags.Any(t => t.Value.Contains(criteria.SearchText ?? ""));

            var totalCount = await dbContext.FinancialTransactions
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var financialTransactions = await dbContext.FinancialTransactions
                .AsNoTracking()
                .Include(t => t.Account)
                .Include(t => t.Tags)
                .Where(predicate)
                .OrderByDescending(t => t.TransactionDate)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(t => t.ToFinancialTransactionInfo())
                .ToListAsync(cancellationToken);

            return new FinancialTransactionSearchResults
            {
                FinancialTransactions = financialTransactions,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<FinancialTransactionInfo> DemandFinancialTransaction(FinancialTransactionId id,
            CancellationToken cancellationToken)
        {
            return await dbContext.FinancialTransactions
                .Include(t => t.Account)
                .Include(t => t.Tags)
                .Where(t => t.Id == id.Value)
                .Select(t => t.ToFinancialTransactionInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task UpdateFinancialTransactionTags(FinancialTransactionId id, IEnumerable<string> tags,
            CancellationToken cancellationToken)
        {
            var data = await dbContext.FinancialTransactions
                .Include(v => v.Tags)
                .SingleAsync(v => v.Id == id.Value, cancellationToken);

            await UpdateTags(data, tags);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<AccountData> GetOrAddAccount(string accountName, CancellationToken cancellationToken)
        {
            var localAccount = dbContext.Accounts.Local
                .Where(a => a.Name == accountName)
                .SingleOrDefault();

            var account = localAccount
                ?? await dbContext.Accounts
                    .Where(a => a.Name == accountName)
                    .SingleOrDefaultAsync(cancellationToken);

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
