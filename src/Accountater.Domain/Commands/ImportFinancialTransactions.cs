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
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;
        private readonly IFinancialTransactionRuleEvaluator ruleEvaluator;

        public ImportFinancialTransactionsHandler(IFinancialTransactionCsvParser financialTransactionCsvParser,
            IFinancialTransactionRepository financialTransactionRepository, IAccountRepository accountRepository,
            IFinancialTransactionMetadataRuleRepository ruleRepository, IFinancialTransactionRuleEvaluator ruleEvaluator)
        {
            this.financialTransactionCsvParser = financialTransactionCsvParser;
            this.financialTransactionRepository = financialTransactionRepository;
            this.accountRepository = accountRepository;
            this.ruleRepository = ruleRepository;
            this.ruleEvaluator = ruleEvaluator;
        }

        public async Task Handle(ImportFinancialTransactions request, CancellationToken cancellationToken)
        {
            var importedTransactions = await financialTransactionCsvParser.Parse(
                request.CsvImportSchemaId, 
                request.CsvFileStream, 
                cancellationToken);

            var account = await accountRepository.DemandAccountInfo(request.AccountId, cancellationToken);
            var rules = await ruleRepository.GetRuleInfos(cancellationToken);

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
                ApplyRules(rules, financialTransaction, financialTransactionInfo);
            }

            await financialTransactionRepository.CreateFinancialTransactions(
                financialTransactions,
                cancellationToken);
        }

        private void ApplyRules(IEnumerable<FinancialTransactionMetadataRuleInfo> rules, FinancialTransaction financialTransaction, 
            FinancialTransactionInfo financialTransactionInfo)
        {
            foreach (var rule in rules)
            {
                if (!financialTransactionInfo.Tags.Contains(rule.MetadataValue)
                    && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                {
                    financialTransaction.Tags.Add(rule.MetadataValue);
                }
            }
        }
    }
}
