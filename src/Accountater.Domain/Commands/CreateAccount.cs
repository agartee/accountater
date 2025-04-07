using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateAccount : IRequest<AccountInfo>
    {
        public required string Name { get; init; }
        public required AccountType Type { get; init; }
        public required string Description { get; init; }
    }

    public class CreateAccountHandler : IRequestHandler<CreateAccount, AccountInfo>
    {
        private readonly IAccountRepository accountRepository;

        public CreateAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<AccountInfo> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Id = AccountId.NewId(),
                Name = request.Name,
                Type = request.Type,
                Description = request.Description
            };

            return await accountRepository.SaveAccount(account, cancellationToken);
        }
    }
}
