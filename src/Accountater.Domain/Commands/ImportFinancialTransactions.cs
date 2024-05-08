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
            var tagRules = await tagRuleRepository.GetAllTagRules(cancellationToken);

            var financialTransactions = new List<FinancialTransaction>();

            foreach (var financialTransactionImport in imports)
            {
                // rich object graph for rule processing
                var financialTransactionInfo = financialTransactionImport.ToFinancialTransactionInfo(
                    FinancialTransactionId.NewId(), account);

                // persisted object
                var financialTransaction = financialTransactionInfo.ToFinancialTransaction();
                financialTransactions.Add(financialTransaction);

                foreach (var tagRule in tagRules)
                {
                    if (!financialTransactionInfo.Tags.Contains(tagRule.Tag)
                        && ruleEvaluator.Evaluate(tagRule.Expression, financialTransactionInfo))
                    {
                        financialTransaction.AddTag(tagRule.Tag);
                    }
                }
            }

            await financialTransactionRepository.InsertFinancialTransactions(financialTransactions,
                cancellationToken);
        }
    }
}
