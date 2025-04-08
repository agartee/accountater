using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionMetadataRuleRepository
    {
        Task<IEnumerable<FinancialTransactionMetadataRuleInfo>> GetRuleInfos(CancellationToken cancellationToken);
        Task<FinancialTransactionMetadataRuleInfo> SaveRule(FinancialTransactionMetadataRule rule, CancellationToken cancellationToken);
        Task<FinancialTransactionMetadataRule> DemandRule(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken);
        Task<FinancialTransactionMetadataRuleInfo> DemandRuleInfo(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken);
        Task DeleteRule(FinancialTransactionMetadataRuleId id, CancellationToken cancellationToken);
        Task<FinancialTransactionMetadataRuleSearchResults> SearchRules(SearchCriteria criteria, CancellationToken cancellationToken);
    }
}
