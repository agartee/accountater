using Accountater.Domain.Extensions;
using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record RunFinancialTransactionMetadataRule : IRequest
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
    }

    public class RunFinancialTransactionMetadataRuleHandler : IRequestHandler<RunFinancialTransactionMetadataRule>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;
        private readonly IFinancialTransactionRuleEvaluator ruleEvaluator;

        public RunFinancialTransactionMetadataRuleHandler(IFinancialTransactionRepository financialTransactionRepository,
            IFinancialTransactionMetadataRuleRepository ruleRepository, IFinancialTransactionRuleEvaluator ruleEvaluator)
        {
            this.financialTransactionRepository = financialTransactionRepository;
            this.ruleRepository = ruleRepository;
            this.ruleEvaluator = ruleEvaluator;
        }
        public async Task Handle(RunFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            var rule = await ruleRepository.DemandRuleInfo(request.Id, cancellationToken);

            var financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                new FinancialTransactionSearchCriteria(), cancellationToken);
            var totalPages = financialTransactionsSearchResults.TotalPages;

            var updatedFinancialTransactions = new List<FinancialTransaction>();
            for (var i = 0; i < totalPages; i++)
            {
                foreach(var financialTransactionInfo in financialTransactionsSearchResults.FinancialTransactions)
                {
                    if(!financialTransactionInfo.Tags.Contains(rule.Tag)
                       && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                    {
                        var financialTransaction = financialTransactionInfo.ToFinancialTransaction();
                        financialTransaction.Tags.Add(rule.Tag);

                        updatedFinancialTransactions.Add(financialTransaction);
                    }
                }

                financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                    new FinancialTransactionSearchCriteria { PageIndex = i + 1 }, cancellationToken);
            }

            await financialTransactionRepository.UpdateFinancialTransactions(updatedFinancialTransactions, cancellationToken);
        }
    }
}
