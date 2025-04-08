using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record DeleteFinancialTransactionMetadataRule : IRequest
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
    }

    public class DeleteFinancialTransactionMetadataRuleHandler : IRequestHandler<DeleteFinancialTransactionMetadataRule>
    {
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;

        public DeleteFinancialTransactionMetadataRuleHandler(IFinancialTransactionMetadataRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        public async Task Handle(DeleteFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            await ruleRepository.DeleteRule(request.Id, cancellationToken);
        }
    }
}
