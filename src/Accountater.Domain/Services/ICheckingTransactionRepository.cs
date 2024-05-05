using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ICheckingTransactionRepository
    {
        Task InsertCheckingTransactions(IEnumerable<CheckingTransaction> transactions);
    }
}
