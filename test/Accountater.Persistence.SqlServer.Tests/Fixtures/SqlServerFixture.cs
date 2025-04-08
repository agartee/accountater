using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Accountater.Persistence.SqlServer.Tests.Fixtures
{
    [ExcludeFromCodeCoverage]
    public class SqlServerFixture
    {
        private readonly IConfiguration config;

        public SqlServerFixture()
        {
            config = new ConfigurationBuilder()
                .AddUserSecrets<SqlServerFixture>()
                .AddEnvironmentVariables()
                .Build();

            var configurationDbContext = CreateDbContext();
            configurationDbContext.Database.EnsureDeleted();
            configurationDbContext.Database.EnsureCreated();
        }

        public AccountaterDbContext CreateDbContext()
        {
            return new AccountaterDbContext(new DbContextOptionsBuilder<AccountaterDbContext>()
                .UseSqlServer(config.GetConnectionString("testDatabase")).Options);
        }

        public async Task Seed(object[] entities, [CallerMemberName] string? caller = null)
        {
            var dbContext = CreateDbContext();

            foreach (var entity in entities)
                dbContext.Add(entity);

            await dbContext.SaveChangesAsync();
        }

        public void ClearData()
        {
            var dbContext = CreateDbContext();

            var sqlCommandList = CreateList(
                new { Order = 3, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{AccountData.TableName}]" },
                new { Order = 2, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{CsvImportSchemaData.TableName}]" },
                new { Order = 1, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{CsvImportSchemaMappingData.TableName}]" },
                new { Order = 3, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{FinancialTransactionData.TableName}]" },
                new { Order = 3, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{TagData.TableName}]" },
                new { Order = 2, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{FinancialTransactionMetadataRuleData.TableName}]" },
                new { Order = 1, SqlCommand = $"DELETE FROM [{AccountaterDbContext.SchemaName}].[{TransactionTagData.TableName}]" });

            var sql = string.Join(Environment.NewLine, sqlCommandList
                .OrderBy(item => item.Order)
                .Select(item => item.SqlCommand)
                .ToArray());

            dbContext.Database.ExecuteSqlRaw(sql);
        }

        private static List<T> CreateList<T>(params T[] elements) => [.. elements];
    }

    [CollectionDefinition("SqlServerTestCollection")]
    public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
