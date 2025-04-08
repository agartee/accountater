using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateCategory : IRequest<CategoryInfo>
    {
        public required CategoryId Id { get; init; }
        public required string Name { get; init; }
    }

    public class UpdateCategoryHandler : IRequestHandler<UpdateCategory, CategoryInfo>
    {
        private readonly ICategoryRepository categoryRepository;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryInfo> Handle(UpdateCategory request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.DemandCategory(request.Id, cancellationToken);

            category.Name = request.Name;

            return await categoryRepository.SaveCategory(category, cancellationToken);
        }
    }
}
