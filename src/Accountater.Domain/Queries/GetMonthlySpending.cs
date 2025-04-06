using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record GetMonthlySpending : IRequest<IEnumerable<MonthlySpendingSummaryInfo>>
    {
        public int? StartMonth { get; init; }
        public int? StartYear { get; init; }
        public int? EndMonth { get; init; }
        public int? EndYear { get; init; }
    }

    public class GetMonthlySpendingHandler : IRequestHandler<GetMonthlySpending, IEnumerable<MonthlySpendingSummaryInfo>>
    {
        private readonly IMonthlySpendingAnalyzer monthlySpendingAnalyzer;

        public GetMonthlySpendingHandler(IMonthlySpendingAnalyzer monthlySpendingAnalyzer)
        {
            this.monthlySpendingAnalyzer = monthlySpendingAnalyzer;
        }

        public async Task<IEnumerable<MonthlySpendingSummaryInfo>> Handle(GetMonthlySpending request, CancellationToken cancellationToken)
        {
            return await monthlySpendingAnalyzer.GetMonthlySpending(new MonthlySpendingCriteria
            {
                StartMonth = request.StartMonth ?? 0,
                StartYear = request.StartYear ?? 0,
                EndMonth = request.EndMonth ?? 0,
                EndYear = request.EndYear ?? 0
            });
        }
    }
}
