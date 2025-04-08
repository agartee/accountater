using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateFinancialTransactionMetadataRule : IRequest<FinancialTransactionMetadataRuleInfo>
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
        public required string Name { get; init; }
        public required string Expression { get; init; }
        public required FinancialTransactionMetadataType MetadataType { get; init; }
        public required string MetadataValue { get; init; }
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

            rule.Name = request.Name;
            rule.Expression = request.Expression;
            rule.MetadataType = request.MetadataType;
            rule.MetadataValue = request.MetadataValue;

            return await ruleRepository.SaveRule(rule, cancellationToken);
        }
    }
}
