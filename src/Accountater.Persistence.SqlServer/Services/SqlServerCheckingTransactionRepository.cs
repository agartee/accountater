using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerCheckingTransactionRepository : ICheckingTransactionRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerCheckingTransactionRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertCheckingTransactions(IEnumerable<CheckingTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var account = await GetOrAddAccount("Checking");

                account.Transactions.Add(new TransactionData
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    TransactionDate = transaction.Date,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                });
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task<AccountData> GetOrAddAccount(string accountName)
        {
            var account = await dbContext.Accounts
                .Where(a => a.Name == accountName)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                account = new AccountData
                {
                    Id = Guid.NewGuid(),
                    Name = accountName,
                };

                dbContext.Accounts.Add(account);
            }

            return account;
        }
    }
}
