using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Categoryater.Domain.Queries
{
    public record ListAllCategories : IRequest<IEnumerable<CategoryInfo>>
    {
    }

    public class ListAllCategoriesHandler : IRequestHandler<ListAllCategories, IEnumerable<CategoryInfo>>
    {
        private readonly ICategoryRepository accountRepository;

        public ListAllCategoriesHandler(ICategoryRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<CategoryInfo>> Handle(ListAllCategories request, CancellationToken cancellationToken)
        {
            return await accountRepository.GetCategoryInfos(cancellationToken);
        }
    }
}
