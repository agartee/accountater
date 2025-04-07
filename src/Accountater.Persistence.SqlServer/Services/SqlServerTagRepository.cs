using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerTagRepository : ITagRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerTagRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteTag(TagId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.Tags
                .SingleOrDefaultAsync(t => t.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.Tags.Remove(data);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Tag> DemandTag(TagId id, CancellationToken cancellationToken)
        {
            return await dbContext.Tags
                .Where(t => t.Id == id.Value)
                .Select(t => t.ToTag())
                .SingleAsync(cancellationToken);
        }

        public async Task<TagInfo> DemandTagInfo(TagId id, CancellationToken cancellationToken)
        {
            return await dbContext.Tags
                .Where(t => t.Id == id.Value)
                .Select(t => t.ToTagInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<TagSearchResults> SearchTags(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<TagData, bool>> predicate = r => criteria.SearchText == null
                || r.Value!.Contains(criteria.SearchText);

            var totalCount = await dbContext.Tags
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var tags = await dbContext.Tags
                .Where(predicate)
                .OrderBy(t => t.Value)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(t => t.ToTagInfo())
                .ToListAsync(cancellationToken);

            return new TagSearchResults
            {
                Tags = tags,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<TagInfo>> GetTagInfos(CancellationToken cancellationToken)
        {
            return await dbContext.Tags
                .OrderBy(t => t.Value)
                .Select(t => t.ToTagInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<TagInfo> SaveTag(Tag tag, CancellationToken cancellationToken)
        {
            var data = await dbContext.Tags
                .SingleOrDefaultAsync(r => r.Id == tag.Id.Value, cancellationToken);

            if (data == null)
                data = CreateTag(tag);
            else
                UpdateTag(tag, data);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToTagInfo();
        }

        private void UpdateTag(Tag tag, TagData data)
        {
            data.Value = tag.Value;
            data.Color = tag.Color;
            data.Order = tag.Order;
        }

        private TagData CreateTag(Tag tag)
        {
            var data = new TagData
            {
                Id = tag.Id.Value,
                Value = tag.Value,
                Color = tag.Color,
                Order = tag.Order
            };

            dbContext.Tags.Add(data);

            return data;
        }
    }
}
