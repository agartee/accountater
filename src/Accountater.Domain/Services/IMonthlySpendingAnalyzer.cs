using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IMonthlySpendingAnalyzer
    {
        Task<IEnumerable<MonthlySpendingSummaryInfo>> GetMonthlySpending(MonthlySpendingCriteria criteria);
    }

    public record MonthlySpendingCriteria
    {
        public int StartMonth { get; init; }
        public int StartYear { get; init; }
        public int EndMonth { get; init; }
        public int EndYear { get; init; }
    }
}
