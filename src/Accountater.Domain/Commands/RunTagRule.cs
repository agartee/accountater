using Accountater.Domain.Extensions;
using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record RunTagRule : IRequest
    {
        public required TagRuleId Id { get; init; }
    }

    public class RunTagRuleHandler : IRequestHandler<RunTagRule>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;
        private readonly ITagRuleRepository tagRuleRepository;
        private readonly IRuleEvaluator ruleEvaluator;

        public RunTagRuleHandler(IFinancialTransactionRepository financialTransactionRepository,
            ITagRuleRepository tagRuleRepository, IRuleEvaluator ruleEvaluator)
        {
            this.financialTransactionRepository = financialTransactionRepository;
            this.tagRuleRepository = tagRuleRepository;
            this.ruleEvaluator = ruleEvaluator;
        }
        public async Task Handle(RunTagRule request, CancellationToken cancellationToken)
        {
            var tagRule = await tagRuleRepository.DemandTagRuleInfo(request.Id, cancellationToken);

            var financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                new FinancialTransactionSearchCriteria(), cancellationToken);
            var totalPages = financialTransactionsSearchResults.TotalPages;

            var updatedFinancialTransactions = new List<FinancialTransaction>();
            for (var i = 0; i < totalPages; i++)
            {
                foreach(var financialTransactionInfo in financialTransactionsSearchResults.FinancialTransactions)
                {
                    if(!financialTransactionInfo.Tags.Contains(tagRule.Tag)
                       && ruleEvaluator.Evaluate(tagRule.Expression, financialTransactionInfo))
                    {
                        var financialTransaction = financialTransactionInfo.ToFinancialTransaction();
                        financialTransaction.Tags.Add(tagRule.Tag);

                        updatedFinancialTransactions.Add(financialTransaction);
                    }
                }

                financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                    new FinancialTransactionSearchCriteria { PageIndex = i + 1 }, cancellationToken);
            }

            await financialTransactionRepository.UpdateFinancialTransactionTags(updatedFinancialTransactions, cancellationToken);
        }
    }
}
