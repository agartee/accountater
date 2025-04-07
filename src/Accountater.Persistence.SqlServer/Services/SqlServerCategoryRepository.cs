using Accountater.Domain.Models;
using Accountater.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerCategoryRepository : ICategoryRepository
    {
        public Task DeleteCategory(CategoryId id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Category> DemandCategory(CategoryId id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryInfo> DemandCategoryInfo(CategoryId id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryInfo>> GetCategoryInfos(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryInfo> SaveCategory(Category category, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CategorySearchResults> SearchCategorys(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
