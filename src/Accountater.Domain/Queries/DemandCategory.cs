using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandCategory : IRequest<CategoryInfo>
    {
        public required CategoryId Id { get; init; }
    }

    public class DemandCategoryHandler : IRequestHandler<DemandCategory, CategoryInfo>
    {
        private readonly ICategoryRepository categoryRepository;

        public DemandCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryInfo> Handle(DemandCategory request, CancellationToken cancellationToken)
        {
            return await categoryRepository.DemandCategoryInfo(request.Id, cancellationToken);
        }
    }
}
