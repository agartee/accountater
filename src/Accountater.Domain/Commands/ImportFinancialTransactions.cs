using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public class ImportFinancialTransactions : IRequest
    {
        public required AccountId AccountId { get; init; }
        public required Stream CsvFileStream { get; init; }
    }

    public class ImportFinancialTransactionsHandler : IRequestHandler<ImportFinancialTransactions>
    {
        private readonly IFinancialTransactionCsvParser financialTransactionCsvParser;
        private readonly IFinancialTransactionRepository financialTransactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ITagRuleRepository tagRuleRepository;
        private readonly IRuleEvaluator ruleEvaluator;

        public ImportFinancialTransactionsHandler(IFinancialTransactionCsvParser financialTransactionCsvParser,
            IFinancialTransactionRepository financialTransactionRepository, IAccountRepository accountRepository,
            ITagRuleRepository tagRuleRepository, IRuleEvaluator ruleEvaluator)
        {
            this.financialTransactionCsvParser = financialTransactionCsvParser;
            this.financialTransactionRepository = financialTransactionRepository;
            this.accountRepository = accountRepository;
            this.tagRuleRepository = tagRuleRepository;
            this.ruleEvaluator = ruleEvaluator;
        }

        public async Task Handle(ImportFinancialTransactions request, CancellationToken cancellationToken)
        {
            var imports = financialTransactionCsvParser.Parse(request.CsvFileStream);
            var account = await accountRepository.DemandAccountInfo(request.AccountId, cancellationToken);
            var tagRules = await tagRuleRepository.GetTagRules(cancellationToken);

            var financialTransactions = new List<FinancialTransaction>();

            foreach (var import in imports)
            {
                var financialTransaction = import.ToFinancialTransaction(
                    FinancialTransactionId.NewId(), account);

                financialTransactions.Add(financialTransaction);

                foreach (var tagRule in tagRules)
                {
                    if (!financialTransaction.Tags.Contains(tagRule.Tag)
                        && ruleEvaluator.Evaluate(tagRule.Expression, financialTransaction))
                    {
                        financialTransaction.Tags.Add(tagRule.Tag);
                    }
                }
            }

            await financialTransactionRepository.InsertFinancialTransactions(financialTransactions,
                cancellationToken);
        }
    }
}
