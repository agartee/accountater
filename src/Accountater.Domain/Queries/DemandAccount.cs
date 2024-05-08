using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandAccount : IRequest<AccountInfo>
    {
        public required AccountId Id { get; init; }
    }

    public class DemandAccountHandler : IRequestHandler<DemandAccount, AccountInfo>
    {
        private readonly IAccountRepository accountRepository;

        public DemandAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<AccountInfo> Handle(DemandAccount request, CancellationToken cancellationToken)
        {
            return await accountRepository.DemandAccountInfo(request.Id, cancellationToken);
        }
    }
}
