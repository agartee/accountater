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
        private readonly ICategoryRepository categoryRepository;
        private readonly IFinancialTransactionMetadataRuleRepository ruleRepository;
        private readonly IFinancialTransactionRuleEvaluator ruleEvaluator;

        public RunFinancialTransactionMetadataRuleHandler(IFinancialTransactionRepository financialTransactionRepository,
            IFinancialTransactionMetadataRuleRepository ruleRepository, IFinancialTransactionRuleEvaluator ruleEvaluator, 
            ICategoryRepository categoryRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
            this.ruleRepository = ruleRepository;
            this.ruleEvaluator = ruleEvaluator;
            this.categoryRepository = categoryRepository;
        }

        public async Task Handle(RunFinancialTransactionMetadataRule request, CancellationToken cancellationToken)
        {
            var rule = await ruleRepository.DemandRuleInfo(request.Id, cancellationToken);

            var financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                new SearchCriteria(), cancellationToken);
            var totalPages = financialTransactionsSearchResults.TotalPages;

            var updatedFinancialTransactions = new List<FinancialTransaction>();
            for (var i = 0; i < totalPages; i++)
            {
                foreach(var financialTransactionInfo in financialTransactionsSearchResults.FinancialTransactions)
                {
                    var financialTransaction = financialTransactionInfo.ToFinancialTransaction();

                    if (rule.MetadataType == FinancialTransactionMetadataType.Tag)
                    {
                        if(!financialTransactionInfo.Tags.Contains(rule.MetadataValue) 
                           && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                        {
                            financialTransaction.Tags.Add(rule.MetadataValue);
                            updatedFinancialTransactions.Add(financialTransaction);
                        }
                    }

                    else if(rule.MetadataType == FinancialTransactionMetadataType.Category)
                    {
                        var category = await CreateCategoryIfNotExists(rule, cancellationToken);

                        if(financialTransactionInfo.Category?.Id != category.Id
                           && ruleEvaluator.Evaluate(rule.Expression, financialTransactionInfo))
                        {
                            financialTransaction.CategoryId = category.Id;
                            updatedFinancialTransactions.Add(financialTransaction);
                        }
                    }
                }

                financialTransactionsSearchResults = await financialTransactionRepository.SearchFinancialTransactions(
                    new SearchCriteria { PageIndex = i + 1 }, cancellationToken);
            }

            await financialTransactionRepository.UpdateFinancialTransactions(updatedFinancialTransactions, cancellationToken);
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
