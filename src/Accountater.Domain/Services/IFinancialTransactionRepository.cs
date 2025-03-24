using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionRepository
    {
        Task InsertFinancialTransactions(IEnumerable<FinancialTransaction> transactions,
            CancellationToken cancellationToken);
        Task<FinancialTransactionSearchResults> SearchFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken);
        Task<FinancialTransactionInfo> DemandFinancialTransaction(FinancialTransactionId id,
            CancellationToken cancellationToken);
        Task UpdateFinancialTransactionTags(FinancialTransactionId id, IEnumerable<string> tags,
            CancellationToken cancellationToken);
    }
}
