using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchFinancialTransactions : IRequest<FinancialTransactionSearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchFinancialTransactionsHandler
        : IRequestHandler<SearchFinancialTransactions, FinancialTransactionSearchResults>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public SearchFinancialTransactionsHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public async Task<FinancialTransactionSearchResults> Handle(
            SearchFinancialTransactions request, CancellationToken cancellationToken)
        {
            return await financialTransactionRepository.SearchFinancialTransactions(
                new SearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
