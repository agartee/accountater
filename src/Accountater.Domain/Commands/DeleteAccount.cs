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
        private readonly IAccountRepository tagRuleRepository;

        public DeleteAccountHandler(IAccountRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task Handle(DeleteAccount request, CancellationToken cancellationToken)
        {
            await tagRuleRepository.DeleteAccount(request.Id, cancellationToken);
        }
    }
}
