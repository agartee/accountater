using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerMonthlyActivityAnalyzer : IMonthlyActivityAnalyzer
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerMonthlyActivityAnalyzer(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MonthlyActivityInfo> GetMonthlyActivity(MonthlySpendingCriteria criteria, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(criteria.StartYear, criteria.StartMonth, 1);
            var endDate = new DateTime(criteria.EndYear, criteria.EndMonth, 1).AddMonths(1).AddDays(-1);

            var transactions = await dbContext.FinancialTransactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToListAsync(cancellationToken);

            var monthlyIncome = transactions
                .Where(t => t.Amount > 0)
                .Where(t => t.Account!.Type == AccountType.Bank)
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new MonthlyIncomeInfo
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToList();

            var monthlyCategorySpending = transactions
                .Where(t => t.Amount < 0)
                .Where(t => t.CategoryId != CategoryId.CreditCardPaymentCategoryId().Value)
                .GroupBy(t => new { t.Date.Year, t.Date.Month, Category = t.Category != null ? t.Category.Name : "Uncategorized" })
                .Select(g => new MonthlyCategorySpendingInfo
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Category = g.Key.Category,
                    Amount = g.Sum(t => Math.Abs(t.Amount))
                })
                .OrderBy(t => t.Year)
                .ThenBy(t => t.Month)
                .ThenBy(t => t.Category)
                .ToList();

            return new MonthlyActivityInfo
            {
                MonthlyIncome = monthlyIncome,
                MonthlyCategorySpending = monthlyCategorySpending
            };
        }
    }
}
