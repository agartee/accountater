using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerTagRuleRepository : ITagRuleRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerTagRuleRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteTagRule(TagRuleId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.TagRules
                .SingleOrDefaultAsync(r => r.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.TagRules.Remove(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TagRule> DemandTagRule(TagRuleId id, CancellationToken cancellationToken)
        {
            return await dbContext.TagRules
                .Include(r => r.Tag)
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToTagRule())
                .SingleAsync(cancellationToken);
        }

        public async Task<TagRuleInfo> DemandTagRuleInfo(TagRuleId id, CancellationToken cancellationToken)
        {
            return await dbContext.TagRules
                .Include(r => r.Tag)
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToTagRuleInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<TagRuleInfo>> GetTagRuleInfos(CancellationToken cancellationToken)
        {
            return await dbContext.TagRules
                .Include(r => r.Tag)
                .OrderBy(r => r.Name)
                .Select(r => r.ToTagRuleInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<TagRuleSearchResults> SearchTagRules(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<TagRuleData, bool>> predicate = r => criteria.SearchText == null
                || r.Tag!.Value.Contains(criteria.SearchText)
                || r.Name.Contains(criteria.SearchText!);

            var totalCount = await dbContext.TagRules
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var tagRules = await dbContext.TagRules
                .Include(r => r.Tag)
                .Where(predicate)
                .OrderBy(r => r.Name)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(r => r.ToTagRuleInfo())
                .ToListAsync(cancellationToken);

            return new TagRuleSearchResults
            {
                TagRules = tagRules,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<TagRuleInfo> SaveTagRule(TagRule tagRule, CancellationToken cancellationToken)
        {
            var data = await dbContext.TagRules
                .Include(r => r.Tag)
                .SingleOrDefaultAsync(r => r.Id == tagRule.Id.Value, cancellationToken);

            if (data == null)
                data = await CreateTagRule(tagRule, cancellationToken);
            else
                await UpdateTagRule(tagRule, data, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToTagRuleInfo();
        }

        private async Task<TagRuleData> CreateTagRule(TagRule tagRule, CancellationToken cancellationToken)
        {
            var tagData = await dbContext.Tags
                .SingleOrDefaultAsync(t => t.Value == tagRule.Tag, cancellationToken);

            var tagId = tagData?.Id ?? Guid.NewGuid();

            var data = new TagRuleData
            {
                Id = tagRule.Id.Value,
                Name = tagRule.Name,
                TagId = tagId,
                Tag = tagData == null
                    ? new TagData { Id = tagId, Value = tagRule.Tag }
                    : null,
                Expression = tagRule.Expression,
            };

            dbContext.TagRules.Add(data);

            return data;
        }

        private async Task UpdateTagRule(TagRule tagRule, TagRuleData data, CancellationToken cancellationToken)
        {
            data.Name = tagRule.Name;
            data.Expression = tagRule.Expression;

            var tagData = await dbContext.Tags
                .SingleOrDefaultAsync(t => t.Value == tagRule.Tag, cancellationToken);
            data.TagId = tagData?.Id ?? Guid.NewGuid();

            if (tagData == null)
            {
                var tag = new TagData { Id = data.TagId, Value = tagRule.Tag };
                dbContext.Tags.Add(tag);
            }
        }
    }
}
