using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateAccount : IRequest<AccountInfo>
    {
        public required AccountId Id { get; init; }
        public string? Name { get; init; }
        public AccountType? Type { get; init; }
        public string? Description { get; init; }
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

            if (request.Name != null)
                account.Name = request.Name;

            if (request.Type.HasValue)
                account.Type = request.Type.Value;

            if(request.Description != null)
                account.Description = request.Description;

            return await accountRepository.SaveAccount(account, cancellationToken);
        }
    }
}
