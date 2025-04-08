using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerFinancialTransactionMetadataRuleRepository : IFinancialTransactionMetadataRuleRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerFinancialTransactionMetadataRuleRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteRule(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.Rules
                .SingleOrDefaultAsync(r => r.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.Rules.Remove(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<FinancialTransactionMetadataRule> DemandRule(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken)
        {
            return await dbContext.Rules
                .Include(r => r.Tag)
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToFinancialTransactionMetadataRule())
                .SingleAsync(cancellationToken);
        }

        public async Task<FinancialTransactionMetadataRuleInfo> DemandRuleInfo(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken)
        {
            return await dbContext.Rules
                .Include(r => r.Tag)
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToFinancialTransactionMetadataRuleInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<FinancialTransactionMetadataRuleInfo>> GetRuleInfos(CancellationToken cancellationToken)
        {
            return await dbContext.Rules
                .Include(r => r.Tag)
                .OrderBy(r => r.Name)
                .Select(r => r.ToFinancialTransactionMetadataRuleInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<FinancialTransactionMetadataRuleSearchResults> SearchRules(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<FinancialTransactionMetadataRuleData, bool>> predicate = r => criteria.SearchText == null
                || r.Tag!.Value.Contains(criteria.SearchText)
                || r.Name.Contains(criteria.SearchText!);

            var totalCount = await dbContext.Rules
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var rules = await dbContext.Rules
                .Include(r => r.Tag)
                .Where(predicate)
                .OrderBy(r => r.Name)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(r => r.ToFinancialTransactionMetadataRuleInfo())
                .ToListAsync(cancellationToken);

            return new FinancialTransactionMetadataRuleSearchResults
            {
                Rules = rules,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<FinancialTransactionMetadataRuleInfo> SaveRule(FinancialTransactionMetadataRule rule, CancellationToken cancellationToken)
        {
            var data = await dbContext.Rules
                .Include(r => r.Tag)
                .SingleOrDefaultAsync(r => r.Id == rule.Id.Value, cancellationToken);

            if (data == null)
                data = await CreateRule(rule, cancellationToken);
            else
                await UpdateRule(rule, data, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToFinancialTransactionMetadataRuleInfo();
        }

        private async Task<FinancialTransactionMetadataRuleData> CreateRule(FinancialTransactionMetadataRule rule, CancellationToken cancellationToken)
        {
            var tagData = await dbContext.Tags
                .SingleOrDefaultAsync(t => t.Value == rule.Tag, cancellationToken);

            var tagId = tagData?.Id ?? Guid.NewGuid();

            var data = new FinancialTransactionMetadataRuleData
            {
                Id = rule.Id.Value,
                Name = rule.Name,
                TagId = tagId,
                Tag = tagData == null
                    ? new TagData { Id = tagId, Value = rule.Tag }
                    : null,
                Expression = rule.Expression,
            };

            dbContext.Rules.Add(data);

            return data;
        }

        private async Task UpdateRule(FinancialTransactionMetadataRule rule, FinancialTransactionMetadataRuleData data, CancellationToken cancellationToken)
        {
            data.Name = rule.Name;
            data.Expression = rule.Expression;

            var tagData = await dbContext.Tags
                .SingleOrDefaultAsync(t => t.Value == rule.Tag, cancellationToken);
            data.TagId = tagData?.Id ?? Guid.NewGuid();

            if (tagData == null)
            {
                var tag = new TagData { Id = data.TagId, Value = rule.Tag };
                dbContext.Tags.Add(tag);
            }
        }
    }
}
