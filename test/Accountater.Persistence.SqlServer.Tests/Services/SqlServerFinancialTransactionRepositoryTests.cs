using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;
using Accountater.Persistence.SqlServer.Services;
using Accountater.Persistence.SqlServer.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer.Tests.Services
{
    [Collection("SqlServerTestCollection")]
    public class SqlServerFinancialTransactionRepositoryTests
    {
        private readonly SqlServerFixture fixture;
        private readonly SqlServerFinancialTransactionRepository repository;

        public SqlServerFinancialTransactionRepositoryTests(SqlServerFixture fixture)
        {
            this.fixture = fixture;
            fixture.ClearData();

            repository = new SqlServerFinancialTransactionRepository(fixture.CreateDbContext());
        }

        [Fact]
        public async Task CreateFinancialTransactions_WithNewData_WritesDatabaseRecords()
        {
            var accountId = AccountId.NewId();
            var account = new AccountData
            {
                Id = accountId.Value,
                Name = "Test Account",
                Type = AccountType.Bank
            };

            await fixture.Seed(account);

            var transactions = new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = FinancialTransactionId.NewId(),
                    AccountId = accountId,
                    Date = DateTime.UtcNow,
                    Description = "Test Transaction 1",
                    Amount = 100,
                    Tags = new List<string> { "Tag1", "Tag2" }
                },
                new FinancialTransaction
                {
                    Id = FinancialTransactionId.NewId(),
                    AccountId = accountId,
                    Date = DateTime.UtcNow,
                    Description = "Test Transaction 2",
                    Amount = 200,
                    Tags = new List<string> { "Tag1" }
                }
            };

            await repository.CreateFinancialTransactions(transactions, CancellationToken.None);

            var dbContext = fixture.CreateDbContext();
            var transaction1Data = await dbContext.FinancialTransactions
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Description == "Test Transaction 1");

            transaction1Data.Should().NotBeNull();
            transaction1Data!.Description.Should().Be("Test Transaction 1");
            transaction1Data.Amount.Should().Be(100);
            transaction1Data.Tags.Should().HaveCount(2);

            var transaction2Data = await dbContext.FinancialTransactions
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Description == "Test Transaction 2");

            transaction2Data.Should().NotBeNull();
            transaction2Data!.Description.Should().Be("Test Transaction 2");
            transaction2Data.Amount.Should().Be(200);
            transaction2Data.Tags.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateFinancialTransaction_UpdatesDatabaseRecords()
        {
            var accountId = AccountId.NewId();
            var transactionId = FinancialTransactionId.NewId();

            var account = new AccountData
            {
                Id = accountId.Value,
                Name = "Test Account",
                Type = AccountType.Bank
            };

            var transaction = new FinancialTransactionData
            {
                Id = transactionId.Value,
                AccountId = accountId.Value,
                Date = DateTime.UtcNow,
                Description = "Test Transaction 1",
                Amount = 100,
                Tags = new List<TagData>
                {
                    new TagData { Id = Guid.NewGuid(), Value = "Tag1" },
                    new TagData { Id = Guid.NewGuid(), Value = "Tag2" }
                }
            };

            await fixture.Seed(account, transaction);

            var updatedTransaction = new FinancialTransaction
            {
                Id = transactionId,
                AccountId = accountId,
                Date = DateTime.UtcNow,
                Description = "Test Transaction 1",
                Amount = 100,
                Tags = new List<string> { "Tag3" }
            };

            await repository.UpdateFinancialTransaction(updatedTransaction, CancellationToken.None);
        }

        [Fact]
        public async Task UpdateFinancialTransactions_UpdatesDatabaseRecords()
        {
            var accountId = AccountId.NewId();
            var transaction1Id = FinancialTransactionId.NewId();
            var transaction2Id = FinancialTransactionId.NewId();
            var tag1Id = TagId.NewId();
            var tag2Id = TagId.NewId();

            var account = new AccountData
            {
                Id = accountId.Value,
                Name = "Test Account",
                Type = AccountType.Bank
            };

            var tag1 = new TagData { Id = tag1Id.Value, Value = "Tag1" };
            var tag2 = new TagData { Id = tag2Id.Value, Value = "Tag2" };

            var transaction1 = new FinancialTransactionData
            {
                Id = transaction1Id.Value,
                AccountId = accountId.Value,
                Date = DateTime.UtcNow,
                Description = "Test Transaction 1",
                Amount = 100,
                Tags = new List<TagData> { tag1, tag2 }
            };

            var transaction2 = new FinancialTransactionData
            {
                Id = transaction2Id.Value,
                AccountId = accountId.Value,
                Date = DateTime.UtcNow,
                Description = "Test Transaction 2",
                Amount = 200,
                Tags = new List<TagData> { tag1, tag2 }
            };

            await fixture.Seed(account, transaction1, transaction2);

            var transactions = new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = transaction1Id,
                    AccountId = accountId,
                    Date = DateTime.UtcNow,
                    Description = "Test Transaction 1",
                    Amount = 100,
                    Tags = new List<string> { "Tag3" }
                },
                new FinancialTransaction
                {
                    Id = transaction2Id,
                    AccountId = accountId,
                    Date = DateTime.UtcNow,
                    Description = "Test Transaction 2",
                    Amount = 200,
                    Tags = new List<string> { "Tag3" }
                }
            };

            await repository.UpdateFinancialTransactions(transactions, CancellationToken.None);
        }
    }
}
