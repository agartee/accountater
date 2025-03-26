using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateTagRule : IRequest<TagRuleInfo>
    {
        public required string Name { get; init; }
        public required string Expression { get; init; }
        public required string Tag { get; init; }
    }

    public class CreateTagRuleHandler : IRequestHandler<CreateTagRule, TagRuleInfo>
    {
        private readonly ITagRuleRepository tagRuleRepository;

        public CreateTagRuleHandler(ITagRuleRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task<TagRuleInfo> Handle(CreateTagRule request, CancellationToken cancellationToken)
        {
            var tagRule = new TagRule
            {
                Id = TagRuleId.NewId(),
                Name = request.Name,
                Expression = request.Expression,
                Tag = request.Tag,
            };

            return await tagRuleRepository.SaveTagRule(tagRule, cancellationToken);
        }
    }
}
