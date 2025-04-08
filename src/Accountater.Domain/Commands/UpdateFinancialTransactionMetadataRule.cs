using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateFinancialTransactionMetadataRule : IRequest<FinancialTransactionMetadataRuleInfo>
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
        public string? Name { get; init; }
        public string? Expression { get; init; }
        public string? Tag { get; init; }
    }

    public class UpdateFinancialTransactionMetadataRuleHandler : IRequestHandler<UpdateFinancialTransactionMetadataRule, FinancialTransactionMetadataRuleInfo>
    {
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;

        public UpdateFinancialTransactionMetadataRuleHandler(IFinancialTransactionMetadataRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        public async Task<FinancialTransactionMetadataRuleInfo> Handle(UpdateFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            var rule = await ruleRepository.DemandRule(request.Id, cancellationToken);

            if (request.Name != null)
                rule.Name = request.Name;

            if (request.Expression != null)
                rule.Expression = request.Expression;

            if (request.Tag != null)
                rule.Tag = request.Tag;

            return await ruleRepository.SaveRule(rule, cancellationToken);
        }
    }
}
