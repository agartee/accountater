using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchCategorys : IRequest<CategorySearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchCategorysHandler
        : IRequestHandler<SearchCategorys, CategorySearchResults>
    {
        private readonly ICategoryRepository categoryRepository;

        public SearchCategorysHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategorySearchResults> Handle(
            SearchCategorys request, CancellationToken cancellationToken)
        {
            return await categoryRepository.SearchCategorys(
                new SearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
