using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ITagRuleRepository
    {
        Task<IEnumerable<TagRuleInfo>> GetTagRuleInfos(CancellationToken cancellationToken);
        Task<TagRuleInfo> SaveTagRule(TagRule tagRule, CancellationToken cancellationToken);
        Task<TagRule> DemandTagRule(TagRuleId id, CancellationToken cancellationToken);
        Task<TagRuleInfo> DemandTagRuleInfo(TagRuleId id, CancellationToken cancellationToken);
        Task DeleteTagRule(TagRuleId id, CancellationToken cancellationToken);
        Task<TagRuleSearchResults> SearchTagRules(SearchCriteria criteria, CancellationToken cancellationToken);
    }
}
