using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionRepository
    {
        Task InsertCheckingTransactions(IEnumerable<CheckingTransaction> transactions);
        Task InsertCreditTransactions(IEnumerable<CreditTransaction> transactions);
        Task<(IEnumerable<FinancialTransactionInfo> FinancialTransactions, int TotalCount)> GetFinancialTransactions(
            FinancialTransactionSearchCriteria criteria, CancellationToken cancellationToken);
        Task UpdateFinancialTransactionTags(FinancialTransactionId id, IEnumerable<string> tags,
            CancellationToken cancellationToken);
    }
}
