using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountInfo>> GetAllAccounts(CancellationToken cancellationToken);
        Task<AccountInfo> SaveAccount(Account account, CancellationToken cancellationToken);
        Task<Account> DemandAccount(AccountId id, CancellationToken cancellationToken);
        Task<AccountInfo> DemandAccountInfo(AccountId id, CancellationToken cancellationToken);
        Task DeleteAccount(AccountId id, CancellationToken cancellationToken);
        Task<AccountSearchResults> SearchAccounts(SearchCriteria criteria, CancellationToken cancellationToken);
    }
}
