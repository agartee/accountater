using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record GetAllAccounts : IRequest<IEnumerable<AccountInfo>>
    {
    }

    public class GetAllAccountsHandler : IRequestHandler<GetAllAccounts, IEnumerable<AccountInfo>>
    {
        private readonly IAccountRepository accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountInfo>> Handle(GetAllAccounts request, CancellationToken cancellationToken)
        {
            return await accountRepository.GetAccounts(cancellationToken);
        }
    }
}
