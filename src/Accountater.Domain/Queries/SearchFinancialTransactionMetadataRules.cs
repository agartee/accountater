using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchFinancialTransactionMetadataRules : IRequest<FinancialTransactionMetadataRuleSearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchFinancialTransactionMetadataRulesHandler
        : IRequestHandler<SearchFinancialTransactionMetadataRules, FinancialTransactionMetadataRuleSearchResults>
    {
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;

        public SearchFinancialTransactionMetadataRulesHandler(IFinancialTransactionMetadataRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        public async Task<FinancialTransactionMetadataRuleSearchResults> Handle(
            SearchFinancialTransactionMetadataRules request, CancellationToken cancellationToken)
        {
            return await ruleRepository.SearchRules(
                new SearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
