using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record ListAllAccounts : IRequest<IEnumerable<AccountInfo>>
    {
    }

    public class ListAllAccountsHandler : IRequestHandler<ListAllAccounts, IEnumerable<AccountInfo>>
    {
        private readonly IAccountRepository accountRepository;

        public ListAllAccountsHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountInfo>> Handle(ListAllAccounts request, CancellationToken cancellationToken)
        {
            return await accountRepository.GetAccounts(cancellationToken);
        }
    }
}
