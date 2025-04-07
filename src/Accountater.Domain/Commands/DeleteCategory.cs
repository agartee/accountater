using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record DeleteCategory : IRequest
    {
        public required CategoryId Id { get; init; }
    }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategory>
    {
        private readonly ICategoryRepository categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            await categoryRepository.DeleteCategory(request.Id, cancellationToken);
        }
    }
}
