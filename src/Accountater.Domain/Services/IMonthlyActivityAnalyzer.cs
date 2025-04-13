using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface IMonthlyActivityAnalyzer
    {
        Task<MonthlyActivityInfo> GetMonthlyActivity(MonthlySpendingCriteria criteria, CancellationToken cancellationToken);
    }

    public record MonthlySpendingCriteria
    {
        public int StartMonth { get; init; }
        public int StartYear { get; init; }
        public int EndMonth { get; init; }
        public int EndYear { get; init; }
    }
}
