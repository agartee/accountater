using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateAccount : IRequest<AccountInfo>
    {
        public required AccountId Id { get; init; }
        public required string Name { get; init; }
        public required AccountType Type { get; init; }
        public required string Description { get; init; }
    }

    public class UpdateAccountHandler : IRequestHandler<UpdateAccount, AccountInfo>
    {
        private readonly IAccountRepository accountRepository;

        public UpdateAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<AccountInfo> Handle(UpdateAccount request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.DemandAccount(request.Id, cancellationToken);

            account.Name = request.Name;
            account.Type = request.Type;
            account.Description = request.Description;

            return await accountRepository.SaveAccount(account, cancellationToken);
        }
    }
}
