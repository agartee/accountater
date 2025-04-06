using System.Globalization;

namespace Accountater.Domain.Models
{
    public record MonthlyActivityInfo
    {
        public IEnumerable<MonthlyCategorySpendingInfo> MonthlyCategorySpending { get; init; } = [];
        public IEnumerable<MonthlyIncomeInfo> MonthlyIncome { get; init; } = [];
    }

    public record MonthlyCategorySpendingInfo
    {
        public required int Month { get; init; }
        public required int Year { get; init; }
        public required string Category { get; init; }
        public required decimal Amount { get; init; }
    }

    public record MonthlyIncomeInfo
    {
        public required int Month { get; init; }
        public required int Year { get; init; }
        public required decimal Amount { get; init; }
    }
}
