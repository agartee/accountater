using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record GetFinancialTransactions
        : IRequest<(IEnumerable<FinancialTransactionInfo> FinancialTransactions, int TotalCount)>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class GetFinancialTransactionsHandler : IRequestHandler<GetFinancialTransactions,
        (IEnumerable<FinancialTransactionInfo> FinancialTransactions, int TotalCount)>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public GetFinancialTransactionsHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public async Task<(IEnumerable<FinancialTransactionInfo> FinancialTransactions, int TotalCount)> Handle(
            GetFinancialTransactions request, CancellationToken cancellationToken)
        {
            return await financialTransactionRepository.GetFinancialTransactions(
                new FinancialTransactionSearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
