using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchTagRules : IRequest<TagRuleSearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchTagRulesHandler
        : IRequestHandler<SearchTagRules, TagRuleSearchResults>
    {
        private readonly ITagRuleRepository tagRuleRepository;

        public SearchTagRulesHandler(ITagRuleRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task<TagRuleSearchResults> Handle(
            SearchTagRules request, CancellationToken cancellationToken)
        {
            return await tagRuleRepository.SearchTagRules(
                new TagRuleSearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
