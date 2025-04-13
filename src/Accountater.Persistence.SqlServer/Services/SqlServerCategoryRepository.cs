using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerCategoryRepository : ICategoryRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerCategoryRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteCategory(CategoryId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.Categories
                .SingleOrDefaultAsync(t => t.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.Categories.Remove(data);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Category> DemandCategory(CategoryId id, CancellationToken cancellationToken)
        {
            return await dbContext.Categories
                .Where(c => c.Id == id.Value)
                .Select(c => c.ToCategory())
                .SingleAsync(cancellationToken);
        }

        public async Task<CategoryInfo> DemandCategoryInfo(CategoryId id, CancellationToken cancellationToken)
        {
            return await dbContext.Categories
                .Where(c => c.Id == id.Value)
                .Select(c => c.ToCategoryInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<CategoryInfo>> GetCategoryInfos(CancellationToken cancellationToken)
        {
            return await dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => c.ToCategoryInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<CategorySearchResults> SearchCategorys(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<CategoryData, bool>> predicate = c => criteria.SearchText == null
                || (criteria.IsExactMatch == true && c.Name.Equals(criteria.SearchText))
                || (criteria.IsExactMatch == false && c.Name.Contains(criteria.SearchText));

            var totalCount = await dbContext.Categories
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var categories = await dbContext.Categories
                .Where(predicate)
                .OrderBy(c => c.Name)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(c => c.ToCategoryInfo())
                .ToListAsync(cancellationToken);

            return new CategorySearchResults
            {
                Categories = categories,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<CategoryInfo> SaveCategory(Category category, CancellationToken cancellationToken)
        {
            var data = await dbContext.Categories
                .SingleOrDefaultAsync(r => r.Id == category.Id.Value, cancellationToken);

            if (data == null)
                data = CreateCategory(category);
            else
                UpdateCategory(category, data);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToCategoryInfo();
        }

        private CategoryData CreateCategory(Category category)
        {
            var data = new CategoryData
            {
                Id = category.Id.Value,
                Name = category.Name,
            };

            dbContext.Categories.Add(data);

            return data;
        }

        private void UpdateCategory(Category category, CategoryData data)
        {
            data.Name = category.Name;
        }   
    }
}
