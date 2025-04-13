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
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToFinancialTransactionMetadataRule())
                .SingleAsync(cancellationToken);
        }

        public async Task<FinancialTransactionMetadataRuleInfo> DemandRuleInfo(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken)
        {
            return await dbContext.Rules
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToFinancialTransactionMetadataRuleInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<FinancialTransactionMetadataRuleInfo>> GetRuleInfos(CancellationToken cancellationToken)
        {
            return await dbContext.Rules
                .OrderBy(r => r.Name)
                .Select(r => r.ToFinancialTransactionMetadataRuleInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<FinancialTransactionMetadataRuleSearchResults> SearchRules(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<FinancialTransactionMetadataRuleData, bool>> predicate = r => criteria.SearchText == null
                || (criteria.IsExactMatch == true && r.MetadataValue.Equals(criteria.SearchText))
                || (criteria.IsExactMatch == false && r.MetadataValue.Contains(criteria.SearchText))
                || (criteria.IsExactMatch == true && r.Name.Equals(criteria.SearchText!))
                || (criteria.IsExactMatch == false && r.Name.Contains(criteria.SearchText!));

            var totalCount = await dbContext.Rules
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var rules = await dbContext.Rules
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
                .SingleOrDefaultAsync(r => r.Id == rule.Id.Value, cancellationToken);

            if (data == null)
                data = CreateRule(rule, cancellationToken);
            else
                UpdateRule(rule, data, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToFinancialTransactionMetadataRuleInfo();
        }

        private FinancialTransactionMetadataRuleData CreateRule(FinancialTransactionMetadataRule rule, CancellationToken cancellationToken)
        {
            var data = new FinancialTransactionMetadataRuleData
            {
                Id = rule.Id.Value,
                Name = rule.Name,
                MetadataType = rule.MetadataType,
                MetadataValue = rule.MetadataValue,
                Expression = rule.Expression,
            };

            dbContext.Rules.Add(data);

            return data;
        }

        private void UpdateRule(FinancialTransactionMetadataRule rule, FinancialTransactionMetadataRuleData data, CancellationToken cancellationToken)
        {
            data.Name = rule.Name;
            data.Expression = rule.Expression;
            data.MetadataType = rule.MetadataType;
            data.MetadataValue = rule.MetadataValue;
        }
    }
}
