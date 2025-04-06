using Accountater.Domain.Models;
using Accountater.Domain.Services;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerMonthlySpendingAnalyzer : IMonthlySpendingAnalyzer
    {
        public async Task<IEnumerable<MonthlySpendingSummaryInfo>> GetMonthlySpending(MonthlySpendingCriteria criteria)
        {
            return new List<MonthlySpendingSummaryInfo>
            {
                new MonthlySpendingSummaryInfo
                {
                    Year = 2023,
                    Month = 1,
                    Income = 5000,
                    CategorySpending = new Dictionary<string, decimal>
                    {
                        { "Food", 200 },
                        { "Transport", 100 },
                        { "Entertainment", 50 }
                    }
                },
                new MonthlySpendingSummaryInfo
                {
                    Year = 2023,
                    Month = 2,
                    Income = 5000,
                    CategorySpending = new Dictionary<string, decimal>
                    {
                        { "Food", 200 },
                        { "Transport", 100 },
                        { "Entertainment", 50 }
                    }
                },
                new MonthlySpendingSummaryInfo
                {
                    Year = 2023,
                    Month = 3,
                    Income = 5000,
                    CategorySpending = new Dictionary<string, decimal>
                    {
                        { "Food", 200 },
                        { "Transport", 100 },
                        { "Entertainment", 50 }
                    }
                }
            }.AsReadOnly();
        }
    }
}
