using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class AccountExtensions
    {
        public static Account ToAccount(this AccountData model)
        {
            return new Account
            {
                Id = new AccountId(model.Id),
                Name = model.Name,
                Description = model.Description
            };
        }

        public static AccountInfo ToAccountInfo(this AccountData model)
        {
            return new AccountInfo
            {
                Id = new AccountId(model.Id),
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}
