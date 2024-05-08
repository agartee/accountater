using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerAccountRepository : IAccountRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerAccountRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteAccount(AccountId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.Accounts
                .SingleOrDefaultAsync(r => r.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.Accounts.Remove(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Account> DemandAccount(AccountId id, CancellationToken cancellationToken)
        {
            return await dbContext.Accounts
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToAccount())
                .SingleAsync(cancellationToken);
        }

        public async Task<AccountInfo> DemandAccountInfo(AccountId id, CancellationToken cancellationToken)
        {
            return await dbContext.Accounts
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToAccountInfo())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<AccountInfo>> GetAllAccounts(CancellationToken cancellationToken)
        {
            return await dbContext.Accounts
                .OrderBy(r => r.Name)
                .Select(r => r.ToAccountInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<AccountSearchResults> SearchAccounts(SearchCriteria criteria, CancellationToken cancellationToken)
        {
            Expression<Func<AccountData, bool>> predicate = r => criteria.SearchText == null
                || r.Name.Contains(criteria.SearchText!);

            var totalCount = await dbContext.Accounts
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);

            var accounts = await dbContext.Accounts
                .Where(predicate)
                .OrderBy(r => r.Name)
                .Skip((criteria.PageIndex) * criteria.PageSize)
                .Take(criteria.PageSize)
                .Select(r => r.ToAccountInfo())
                .ToListAsync(cancellationToken);

            return new AccountSearchResults
            {
                Accounts = accounts,
                SearchText = criteria.SearchText,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                TotalCount = totalCount
            };
        }

        public async Task<AccountInfo> SaveAccount(Account account, CancellationToken cancellationToken)
        {
            var data = await dbContext.Accounts
                .SingleOrDefaultAsync(r => r.Id == account.Id.Value, cancellationToken);

            if (data == null)
                data = CreateAccount(account);
            else
                UpdateAccount(account, data);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToAccountInfo();
        }

        private AccountData CreateAccount(Account account)
        {
            var data = new AccountData
            {
                Id = account.Id.Value,
                Name = account.Name,
                Description = account.Description,
            };

            dbContext.Accounts.Add(data);

            return data;
        }

        private void UpdateAccount(Account account, AccountData data)
        {
            data.Name = account.Name;
            data.Description = account.Description;
        }
    }
}
