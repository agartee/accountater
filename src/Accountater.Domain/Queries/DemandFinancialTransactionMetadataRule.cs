using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandFinancialTransactionMetadataRule : IRequest<FinancialTransactionMetadataRuleInfo>
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
    }

    public class DemandFinancialTransactionMetadataRuleHandler : IRequestHandler<DemandFinancialTransactionMetadataRule, FinancialTransactionMetadataRuleInfo>
    {
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;

        public DemandFinancialTransactionMetadataRuleHandler(IFinancialTransactionMetadataRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        public async Task<FinancialTransactionMetadataRuleInfo> Handle(DemandFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            return await ruleRepository.DemandRuleInfo(request.Id, cancellationToken);
        }
    }
}
