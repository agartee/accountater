using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryInfo>> GetCategoryInfos(CancellationToken cancellationToken);
        Task<CategoryInfo> SaveCategory(Category category, CancellationToken cancellationToken);
        Task<Category> DemandCategory(CategoryId id, CancellationToken cancellationToken);
        Task<CategoryInfo> DemandCategoryInfo(CategoryId id, CancellationToken cancellationToken);
        Task DeleteCategory(CategoryId id, CancellationToken cancellationToken);
        Task<CategorySearchResults> SearchCategorys(SearchCriteria criteria, CancellationToken cancellationToken);
    }
}
