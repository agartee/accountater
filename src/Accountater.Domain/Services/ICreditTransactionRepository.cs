using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICreditTransactionRepository
    {
        Task InsertCreditTransactions(IEnumerable<CreditTransaction> transactions);
    }
}
