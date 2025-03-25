using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record DeleteAccount : IRequest
    {
        public required AccountId Id { get; init; }
    }

    public class DeleteAccountHandler : IRequestHandler<DeleteAccount>
    {
        private readonly IAccountRepository accountRepository;

        public DeleteAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task Handle(DeleteAccount request, CancellationToken cancellationToken)
        {
            await accountRepository.DeleteAccount(request.Id, cancellationToken);
        }
    }
}
