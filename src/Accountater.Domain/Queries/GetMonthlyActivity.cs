using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record GetMonthlyActivity : IRequest<MonthlyActivityInfo>
    {
        public int? StartMonth { get; init; }
        public int? StartYear { get; init; }
        public int? EndMonth { get; init; }
        public int? EndYear { get; init; }
    }

    public class GetMonthlyActivityHandler : IRequestHandler<GetMonthlyActivity, MonthlyActivityInfo>
    {
        private readonly IMonthlyActivityAnalyzer monthlySpendingAnalyzer;

        public GetMonthlyActivityHandler(IMonthlyActivityAnalyzer monthlySpendingAnalyzer)
        {
            this.monthlySpendingAnalyzer = monthlySpendingAnalyzer;
        }

        public async Task<MonthlyActivityInfo> Handle(GetMonthlyActivity request, CancellationToken cancellationToken)
        {
            return await monthlySpendingAnalyzer.GetMonthlyActivity(new MonthlySpendingCriteria
            {
                StartMonth = request.StartMonth ?? 0,
                StartYear = request.StartYear ?? 0,
                EndMonth = request.EndMonth ?? 0,
                EndYear = request.EndYear ?? 0
            });
        }
    }
}
