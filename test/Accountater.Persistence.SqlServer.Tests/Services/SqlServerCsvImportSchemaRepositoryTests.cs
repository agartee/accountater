using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;
using Accountater.Persistence.SqlServer.Services;
using Accountater.Persistence.SqlServer.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer.Tests.Services
{
    [Collection("SqlServerTestCollection")]
    public class SqlServerCsvImportSchemaRepositoryTests
    {
        private readonly SqlServerFixture fixture;
        private readonly SqlServerCsvImportSchemaRepository repository;

        public SqlServerCsvImportSchemaRepositoryTests(SqlServerFixture fixture)
        {
            this.fixture = fixture;
            fixture.ClearData();

            repository = new SqlServerCsvImportSchemaRepository(fixture.CreateDbContext());
        }

        [Fact]
        public async Task SaveImportSchema_WithNewData_ReturnsModelAndWritesDatabaseRecords()
        {
            var id = CsvImportSchemaId.NewId();
            var config = new CsvImportSchema
            {
                Id = id,
                Name = "TestSchema",
                Mappings = [
                    new CsvImportSchemaMapping
                    {
                        MappedProperty = "Date",
                        ColumnIndex = 1
                    },
                    new CsvImportSchemaMapping
                    {
                        MappedProperty = "Amount",
                        ColumnIndex = 2
                    },
                    new CsvImportSchemaMapping
                    {
                        MappedProperty = "Description",
                        ColumnIndex = 0
                    }
                ]
            };

            var result = await repository.SaveImportSchema(config, CancellationToken.None);

            result.Id.Should().Be(id);
            result.Name.Should().Be("TestSchema");
            result.Mappings.Should().HaveCount(3);
            result.Mappings.First(m => m.MappedProperty == "Description").ColumnIndex.Should().Be(0);
            result.Mappings.First(m => m.MappedProperty == "Date").ColumnIndex.Should().Be(1);
            result.Mappings.First(m => m.MappedProperty == "Amount").ColumnIndex.Should().Be(2);

            var dbContext = fixture.CreateDbContext();
            var data = await dbContext.CsvImportSchemas
                .Include(r => r.Mappings)
                .SingleAsync(r => r.Id == config.Id.Value);

            data.Name.Should().Be("TestSchema");
            data.Mappings.Should().HaveCount(3);
            data.Mappings.First(m => m.MappedProperty == "Description").ColumnIndex.Should().Be(0);
            data.Mappings.First(m => m.MappedProperty == "Date").ColumnIndex.Should().Be(1);
            data.Mappings.First(m => m.MappedProperty == "Amount").ColumnIndex.Should().Be(2);
        }

        [Fact]
        public async Task Delete_DeletesDatabaseRecords()
        {
            var id = CsvImportSchemaId.NewId();
            var config = new CsvImportSchemaData
            {
                Id = id.Value,
                Name = "TestSchema",
                Mappings = [new CsvImportSchemaMappingData
                {
                    ImportSchemaId = id.Value,
                    MappedProperty = "Prop1",
                    ColumnIndex = 0
                }]
            };

            await fixture.Seed([config]);

            await repository.DeleteImportMap(id, CancellationToken.None);

            var dbContext = fixture.CreateDbContext();
            var data = await dbContext.CsvImportSchemas
                .Include(r => r.Mappings)
                .SingleOrDefaultAsync(r => r.Id == id.Value);

            data.Should().BeNull();
        }

        [Fact]
        public async Task DemandCsvImportSchema_ReturnsExpectedModel()
        {
            var id = CsvImportSchemaId.NewId();

            var config = new CsvImportSchemaData
            {
                Id = id.Value,
                Name = "TestSchema",
                Mappings = [new CsvImportSchemaMappingData
                {
                    ImportSchemaId = id.Value,
                    MappedProperty = "Prop1",
                    ColumnIndex = 0
                }]
            };

            await fixture.Seed([config]);

            var result = await repository.DemandCsvImportSchema(id, CancellationToken.None);

            result.Id.Should().Be(id);
            result.Name.Should().Be("TestSchema");
            result.Mappings.Should().HaveCount(1);
            result.Mappings.First(m => m.MappedProperty == "Prop1").ColumnIndex.Should().Be(0);
        }
    }
}
