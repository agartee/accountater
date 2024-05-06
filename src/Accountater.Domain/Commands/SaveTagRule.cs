using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record SaveTagRule : IRequest<TagRuleInfo>
    {
        public required TagRuleId Id { get; init; }
        public string? Name { get; init; }
        public string? Expression { get; init; }
        public string? Tag { get; init; }
    }

    public class SaveTagRuleHandler : IRequestHandler<SaveTagRule, TagRuleInfo>
    {
        private readonly ITagRuleRepository tagRuleRepository;

        public SaveTagRuleHandler(ITagRuleRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task<TagRuleInfo> Handle(SaveTagRule request, CancellationToken cancellationToken)
        {
            var tagRule = await tagRuleRepository.DemandTagRule(request.Id, cancellationToken);

            if (request.Name != null)
                tagRule.Name = request.Name;

            if (request.Expression != null)
                tagRule.Expression = request.Expression;

            if (request.Tag != null)
                tagRule.Tag = request.Tag;

            return await tagRuleRepository.SaveTagRule(tagRule, cancellationToken);
        }
    }
}
