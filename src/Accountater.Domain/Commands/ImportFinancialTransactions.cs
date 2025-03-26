using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public class ImportFinancialTransactions : IRequest
    {
        public required AccountId AccountId { get; init; }
        public required CsvImportSchemaId CsvImportSchemaId { get; init; }
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
            var importedTransactions = await financialTransactionCsvParser.Parse(
                request.CsvImportSchemaId, 
                request.CsvFileStream, 
                cancellationToken);

            var account = await accountRepository.DemandAccountInfo(request.AccountId, cancellationToken);
            var tagRules = await tagRuleRepository.GetTagRuleInfos(cancellationToken);

            var financialTransactions = new List<FinancialTransaction>();

            foreach (var importedTransaction in importedTransactions)
            {
                // persisted object
                var financialTransaction = importedTransaction.ToFinancialTransaction(
                    FinancialTransactionId.NewId(), request.AccountId);

                // rich object graph for rule processing
                var financialTransactionInfo = importedTransaction.ToFinancialTransactionInfo(
                    FinancialTransactionId.NewId(), account);

                financialTransactions.Add(financialTransaction);
                ApplyTagRules(tagRules, financialTransaction, financialTransactionInfo);
            }

            await financialTransactionRepository.CreateFinancialTransactions(
                financialTransactions,
                cancellationToken);
        }

        private void ApplyTagRules(IEnumerable<TagRuleInfo> tagRules, FinancialTransaction financialTransaction, 
            FinancialTransactionInfo financialTransactionInfo)
        {
            foreach (var tagRule in tagRules)
            {
                if (!financialTransactionInfo.Tags.Contains(tagRule.Tag)
                    && ruleEvaluator.Evaluate(tagRule.Expression, financialTransactionInfo))
                {
                    financialTransaction.Tags.Add(tagRule.Tag);
                }
            }
        }
    }
}
