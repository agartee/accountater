using System.Globalization;

namespace Accountater.Domain.Models
{
    public class MonthlySpendingSummaryInfo
    {
        public required int Month { get; init; }
        public required int Year { get; init; }
        public required decimal Income { get; init; }
        public required IDictionary<string, decimal> CategorySpending { get; init; } = new Dictionary<string, decimal>().AsReadOnly();
        public string MonthName => DateTimeFormatInfo.CurrentInfo.GetMonthName(Month);
    }
}
