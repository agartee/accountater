using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateTag : IRequest<TagInfo>
    {
        public required string Value { get; init; }
        public string? Color { get; init; }
        public decimal? Order { get; init; }
    }

    public class CreateTagHandler : IRequestHandler<CreateTag, TagInfo>
    {
        private readonly ITagRepository tagRepository;

        public CreateTagHandler(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<TagInfo> Handle(CreateTag request, CancellationToken cancellationToken)
        {
            var tag = new Tag
            {
                Id = TagId.NewId(),
                Value = request.Value,
                Color = request.Color,
                Order = request.Order,
            };

            return await tagRepository.SaveTag(tag, cancellationToken);
        }
    }
}
