using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateCategory : IRequest<CategoryInfo>
    {
        public required string Value { get; init; }
        public required string Name { get; init; }
    }

    public class CreateCategoryHandler : IRequestHandler<CreateCategory, CategoryInfo>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryInfo> Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = CategoryId.NewId(),
                Name = request.Name
            };

            return await categoryRepository.SaveCategory(category, cancellationToken);
        }
    }
}
