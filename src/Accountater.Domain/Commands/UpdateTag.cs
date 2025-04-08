using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateTag : IRequest<TagInfo>
    {
        public required TagId Id { get; init; }
        public required string Value { get; init; }
        public required string Color { get; init; }
        public required decimal Order { get; init; }
    }

    public class UpdateTagHandler : IRequestHandler<UpdateTag, TagInfo>
    {
        private readonly ITagRepository tagRepository;

        public UpdateTagHandler(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<TagInfo> Handle(UpdateTag request, CancellationToken cancellationToken)
        {
            var tag = await tagRepository.DemandTag(request.Id, cancellationToken);

            tag.Value = request.Value;
            tag.Color = request.Color;
            tag.Order = request.Order;

            return await tagRepository.SaveTag(tag, cancellationToken);
        }
    }
}
