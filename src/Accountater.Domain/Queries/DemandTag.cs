using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandTag : IRequest<TagInfo>
    {
        public required TagId Id { get; init; }
    }

    public class DemandTagHandler : IRequestHandler<DemandTag, TagInfo>
    {
        private readonly ITagRepository tagRepository;

        public DemandTagHandler(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<TagInfo> Handle(DemandTag request, CancellationToken cancellationToken)
        {
            return await tagRepository.DemandTagInfo(request.Id, cancellationToken);
        }
    }
}
