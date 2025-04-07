using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record DeleteTag : IRequest
    {
        public required TagId Id { get; init; }
    }

    public class DeleteTagHandler : IRequestHandler<DeleteTag>
    {
        private readonly ITagRepository tagRepository;

        public DeleteTagHandler(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task Handle(DeleteTag request, CancellationToken cancellationToken)
        {
            await tagRepository.DeleteTag(request.Id, cancellationToken);
        }
    }
}
