using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record CreateFinancialTransactionMetadataRule : IRequest<FinancialTransactionMetadataRuleInfo>
    {
        public required string Name { get; init; }
        public required string Expression { get; init; }
        public required string Tag { get; init; }
    }

    public class CreateFinancialTransactionMetadataRuleHandler : IRequestHandler<CreateFinancialTransactionMetadataRule, FinancialTransactionMetadataRuleInfo>
    {
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;

        public CreateFinancialTransactionMetadataRuleHandler(IFinancialTransactionMetadataRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        public async Task<FinancialTransactionMetadataRuleInfo> Handle(CreateFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            var rule = new FinancialTransactionMetadataRule
            {
                Id = FinancialTransactionMetadataRuleId.NewId(),
                Name = request.Name,
                Expression = request.Expression,
                Tag = request.Tag,
            };

            return await ruleRepository.SaveRule(rule, cancellationToken);
        }
    }
}
