using Accountater.Domain.Extensions;
using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;
using System.Threading;

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
        private readonly ICategoryRepository categoryRepository;

        public ImportFinancialTransactionsHandler(IFinancialTransactionCsvParser financialTransactionCsvParser,
            IFinancialTransactionRepository financialTransactionRepository, IAccountRepository accountRepository,
            IFinancialTransactionMetadataRuleRepository ruleRepository, IFinancialTransactionRuleEvaluator ruleEvaluator, ICategoryRepository categoryRepository)
        {
            this.financialTransactionCsvParser = financialTransactionCsvParser;
            this.financialTransactionRepository = financialTransactionRepository;
            this.accountRepository = accountRepository;
            this.ruleRepository = ruleRepository;
            this.ruleEvaluator = ruleEvaluator;
            this.categoryRepository = categoryRepository;
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
                await ApplyRules(rules, financialTransaction, financialTransactionInfo, cancellationToken);
            }

            await financialTransactionRepository.CreateFinancialTransactions(
                financialTransactions,
                cancellationToken);
        }

        private async Task ApplyRules(IEnumerable<FinancialTransactionMetadataRuleInfo> rules, FinancialTransaction financialTransaction, 
            FinancialTransactionInfo financialTransactionInfo, CancellationToken cancellationToken)
        {
            foreach (var rule in rules)
            {
                if (rule.MetadataType == FinancialTransactionMetadataType.Tag)
                {
                    if (!financialTransactionInfo.Tags.Contains(rule.MetadataValue)
                        && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                    {
                        financialTransaction.Tags.Add(rule.MetadataValue);
                    }
                }

                if (rule.MetadataType == FinancialTransactionMetadataType.Category)
                {
                    var category = await CreateCategoryIfNotExists(rule, cancellationToken);

                    if (financialTransactionInfo.Category?.Id != category.Id
                       && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                    {
                        financialTransaction.CategoryId = category.Id;
                    }
                }
            }
        }

        private async Task<CategoryInfo> CreateCategoryIfNotExists(FinancialTransactionMetadataRuleInfo rule, CancellationToken cancellationToken)
        {
            var categorySearchResults = await categoryRepository.SearchCategorys(
                new SearchCriteria { SearchText = rule.MetadataValue, IsExactMatch = true }, cancellationToken);

            var categoryInfo = categorySearchResults.Categories.FirstOrDefault();

            if (categoryInfo == null)
            {
                var category = new Category
                {
                    Id = new CategoryId(Guid.NewGuid()),
                    Name = rule.MetadataValue,
                };

                await categoryRepository.SaveCategory(category, cancellationToken);
                categoryInfo = category.ToCategoryInfo();
            }

            return categoryInfo;
        }
    }
}
