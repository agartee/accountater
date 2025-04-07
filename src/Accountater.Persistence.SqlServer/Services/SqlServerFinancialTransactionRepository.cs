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

        public async Task CreateFinancialTransactions(IEnumerable<FinancialTransaction> transactions,
            CancellationToken cancellationToken)
        {
            var tags = await GetAndStageMissingTags(transactions);

            foreach (var financialTransaction in transactions)
            {
                dbContext.FinancialTransactions.Add(new FinancialTransactionData
                {
                    Id = financialTransaction.Id.Value,
                    AccountId = financialTransaction.AccountId.Value,
                    Date = financialTransaction.Date,
                    Description = financialTransaction.Description,
                    Amount = financialTransaction.Amount,
                    Tags = tags
                        .Where(t => financialTransaction.Tags.Contains(t.Value))
                        .ToList()
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<IEnumerable<TagData>> GetAndStageMissingTags(IEnumerable<FinancialTransaction> transactions)
        {
            var tags = transactions
                .SelectMany(x => x.Tags)
                .Distinct()
                .ToList();

            var existingTags = await dbContext.Tags
                .Where(t => tags.Contains(t.Value))
                .ToListAsync();

            var newTags = tags
                .Where(t => !existingTags.Any(et => et.Value == t))
                .Select(t => new TagData
                {
                    Id = Guid.NewGuid(),
                    Value = t.Truncate(200)
                })
                .ToList();

            foreach (var newTag in newTags)
                dbContext.Tags.Add(newTag);

            return existingTags.Concat(newTags);
        }

        public async Task<FinancialTransactionSearchResults> SearchFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken)
        {
            var amountFilter = ParseAmountFilter(criteria.SearchText);

            Expression<Func<FinancialTransactionData, bool>> predicate = t => criteria.SearchText == null
                || t.Tags.Any(t => t.Value.Contains(criteria.SearchText ?? ""))
                || (amountFilter.HasValue && (t.Amount == amountFilter.Value || t.Amount == -amountFilter.Value))
                || t.Description.Contains(criteria.SearchText!);

            var totalCount = await dbContext.FinancialTransactions
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var financialTransactions = await dbContext.FinancialTransactions
                .AsNoTracking()
                .Include(t => t.Account)
                .Include(t => t.Tags)
                .Where(predicate)
                .OrderByDescending(t => t.Date)
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

        private static decimal? ParseAmountFilter(string? searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var cleaned = searchText.Replace("$", "").Trim();
                if (decimal.TryParse(cleaned, out var parsedAmount))
                    return parsedAmount;
            }

            return null;
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

        public async Task UpdateFinancialTransactionTags(IEnumerable<FinancialTransaction> transactions,
            CancellationToken cancellationToken)
        {
            var tags = await GetAndStageMissingTags(transactions);
            var existingFinancialTransactions = dbContext.FinancialTransactions
                .Include(t => t.Tags)
                .Where(t => transactions.Select(ft => ft.Id.Value).Contains(t.Id))
                .ToList();

            foreach (var financialTransaction in transactions)
            {
                var data = existingFinancialTransactions.Single(t => t.Id == financialTransaction.Id.Value);
                await UpdateTags(data, financialTransaction.Tags);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
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
