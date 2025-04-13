using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Models;
using Accountater.Persistence.SqlServer.Services;
using Accountater.Persistence.SqlServer.Tests.Fixtures;
using FluentAssertions;

namespace Accountater.Persistence.SqlServer.Tests.Services
{
    [Collection("SqlServerTestCollection")]
    public class SqlServerMonthlyActivityAnalyzerTests
    {
        private readonly SqlServerFixture fixture;
        private readonly SqlServerMonthlyActivityAnalyzer analyzer;

        public SqlServerMonthlyActivityAnalyzerTests(SqlServerFixture fixture)
        {
            this.fixture = fixture;
            fixture.ClearData();

            analyzer = new SqlServerMonthlyActivityAnalyzer(fixture.CreateDbContext());
        }

        [Fact]
        public async Task GetMonthlyActivity_ReturnsExpectedResults()
        {
            var accountId = Guid.NewGuid();
            var incomeCategoryId = Guid.NewGuid();
            var rentCategoryId = Guid.NewGuid();
            var groceriesCategoryId = Guid.NewGuid();

            var seedData = new object[]
            {
                new AccountData
                {
                    Id = accountId,
                    Name = "Test Account",
                    Type = AccountType.Bank
                },
                new CategoryData
                {
                    Id = incomeCategoryId,
                    Name = "Income"
                },
                new CategoryData
                {
                    Id = rentCategoryId,
                    Name = "Rent"
                },
                new CategoryData
                {
                    Id = groceriesCategoryId,
                    Name = "Groceries"
                },
                new FinancialTransactionData
                {
                    Id = FinancialTransactionId.NewId().Value,
                    AccountId = accountId,
                    Date = new DateTime(2025, 1, 1),
                    Description = "Paycheck",
                    Amount = 10000,
                    CategoryId = incomeCategoryId,
                },
                new FinancialTransactionData
                {
                    Id = FinancialTransactionId.NewId().Value,
                    AccountId = accountId,
                    Date = new DateTime(2025, 1, 2),
                    Description = "Rent",
                    Amount = -1200,
                    CategoryId = rentCategoryId
                },
                new FinancialTransactionData
                {
                    Id = FinancialTransactionId.NewId().Value,
                    AccountId = accountId,
                    Date = new DateTime(2025, 1, 3),
                    Description = "Some store",
                    Amount = -200,
                    CategoryId = groceriesCategoryId
                },
                new FinancialTransactionData
                {
                    Id = FinancialTransactionId.NewId().Value,
                    AccountId = accountId,
                    Date = new DateTime(2025, 1, 3),
                    Description = "Some other store",
                    Amount = -300,
                    CategoryId = groceriesCategoryId
                },
                new FinancialTransactionData
                {
                    Id = FinancialTransactionId.NewId().Value,
                    AccountId = accountId,
                    Date = new DateTime(2025, 1, 3),
                    Description = "Some clothing store",
                    Amount = -100
                }
            };

            await fixture.Seed(seedData);

            var result = await analyzer.GetMonthlyActivity(new MonthlySpendingCriteria
            {
                StartMonth = 1,
                StartYear = 2025,
                EndMonth = 6,
                EndYear = 2025,
            });

            result.MonthlyIncome.Should().BeEquivalentTo(new[]
            {
                new MonthlyIncomeInfo { Month = 1, Year = 2025, Amount = 10000 }
            });

            // todo: add assertions
        }
    }
}
