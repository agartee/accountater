using Accountater.Domain.Models;
using Accountater.Domain.Services;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerMonthlyActivityAnalyzer : IMonthlyActivityAnalyzer
    {
        public async Task<MonthlyActivityInfo> GetMonthlyActivity(MonthlySpendingCriteria criteria)
        {
            return new MonthlyActivityInfo
            {
                MonthlyIncome =
                [
                    new MonthlyIncomeInfo { Month = 1, Year = 2025, Amount = 2200 },
                    new MonthlyIncomeInfo { Month = 2, Year = 2025, Amount = 1200 },
                    new MonthlyIncomeInfo { Month = 3, Year = 2025, Amount = 2200 },
                    new MonthlyIncomeInfo { Month = 4, Year = 2025, Amount = 2500 },
                    new MonthlyIncomeInfo { Month = 5, Year = 2025, Amount = 2200 },
                    new MonthlyIncomeInfo { Month = 6, Year = 2025, Amount = 3200 }
                ],

                MonthlyCategorySpending =
                [
                    new MonthlyCategorySpendingInfo { Month = 1, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 1, Year = 2025, Category = "Other", Amount = 200 },
                    new MonthlyCategorySpendingInfo { Month = 1, Year = 2025, Category = "Groceries", Amount = 400 },
                    new MonthlyCategorySpendingInfo { Month = 1, Year = 2025, Category = "Dining", Amount = 150 },
                    new MonthlyCategorySpendingInfo { Month = 1, Year = 2025, Category = "Entertainment", Amount = 100 },

                    new MonthlyCategorySpendingInfo { Month = 2, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 2, Year = 2025, Category = "Other", Amount = 200 },
                    new MonthlyCategorySpendingInfo { Month = 2, Year = 2025, Category = "Groceries", Amount = 350 },
                    new MonthlyCategorySpendingInfo { Month = 2, Year = 2025, Category = "Dining", Amount = 200 },
                    new MonthlyCategorySpendingInfo { Month = 2, Year = 2025, Category = "Entertainment", Amount = 80 },

                    new MonthlyCategorySpendingInfo { Month = 3, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 3, Year = 2025, Category = "Other", Amount = 0 },
                    new MonthlyCategorySpendingInfo { Month = 3, Year = 2025, Category = "Groceries", Amount = 370 },
                    new MonthlyCategorySpendingInfo { Month = 3, Year = 2025, Category = "Dining", Amount = 180 },
                    new MonthlyCategorySpendingInfo { Month = 3, Year = 2025, Category = "Entertainment", Amount = 90 },

                    new MonthlyCategorySpendingInfo { Month = 4, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 4, Year = 2025, Category = "Other", Amount = 0 },
                    new MonthlyCategorySpendingInfo { Month = 4, Year = 2025, Category = "Groceries", Amount = 390 },
                    new MonthlyCategorySpendingInfo { Month = 4, Year = 2025, Category = "Dining", Amount = 220 },
                    new MonthlyCategorySpendingInfo { Month = 4, Year = 2025, Category = "Entertainment", Amount = 70 },

                    new MonthlyCategorySpendingInfo { Month = 5, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 5, Year = 2025, Category = "Other", Amount = 0 },
                    new MonthlyCategorySpendingInfo { Month = 5, Year = 2025, Category = "Groceries", Amount = 390 },
                    new MonthlyCategorySpendingInfo { Month = 5, Year = 2025, Category = "Dining", Amount = 220 },
                    new MonthlyCategorySpendingInfo { Month = 5, Year = 2025, Category = "Entertainment", Amount = 70 },

                    new MonthlyCategorySpendingInfo { Month = 6, Year = 2025, Category = "Rent", Amount = 1200 },
                    new MonthlyCategorySpendingInfo { Month = 6, Year = 2025, Category = "Other", Amount = 0 },
                    new MonthlyCategorySpendingInfo { Month = 6, Year = 2025, Category = "Groceries", Amount = 390 },
                    new MonthlyCategorySpendingInfo { Month = 6, Year = 2025, Category = "Dining", Amount = 220 },
                    new MonthlyCategorySpendingInfo { Month = 6, Year = 2025, Category = "Entertainment", Amount = 70 }
                ]
            };

        }
    }
}
