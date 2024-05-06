using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandTagRule : IRequest<TagRuleInfo>
    {
        public required TagRuleId Id { get; init; }
    }

    public class DemandTagRuleHandler : IRequestHandler<DemandTagRule, TagRuleInfo>
    {
        private readonly ITagRuleRepository tagRuleRepository;

        public DemandTagRuleHandler(ITagRuleRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task<TagRuleInfo> Handle(DemandTagRule request, CancellationToken cancellationToken)
        {
            return await tagRuleRepository.DemandTagRuleInfo(request.Id, cancellationToken);
        }
    }
}
