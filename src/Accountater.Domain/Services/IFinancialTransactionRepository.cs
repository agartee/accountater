using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionRepository
    {
        Task CreateFinancialTransactions(IEnumerable<FinancialTransaction> transactions,
            CancellationToken cancellationToken);
        Task UpdateFinancialTransactions(IEnumerable<FinancialTransaction> transactions,
            CancellationToken cancellationToken);
        Task<FinancialTransactionSearchResults> SearchFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken);
        Task<FinancialTransactionInfo> DemandFinancialTransactionInfo(FinancialTransactionId id,
            CancellationToken cancellationToken);
        Task<FinancialTransaction> DemandFinancialTransaction(FinancialTransactionId id,
            CancellationToken cancellationToken);
        Task UpdateFinancialTransaction(FinancialTransaction financialTransaction, CancellationToken cancellationToken);
    }
}
