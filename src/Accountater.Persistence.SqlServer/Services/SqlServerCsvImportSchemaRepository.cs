using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer.Extensions;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer.Services
{
    public class SqlServerCsvImportSchemaRepository : ICsvImportSchemaRepository
    {
        private readonly AccountaterDbContext dbContext;

        public SqlServerCsvImportSchemaRepository(AccountaterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteImportMap(CsvImportSchemaId id, CancellationToken cancellationToken)
        {
            var data = await dbContext.CsvImportSchemas
                .SingleOrDefaultAsync(r => r.Id == id.Value, cancellationToken);

            if (data == null)
                return;

            dbContext.CsvImportSchemas.Remove(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CsvImportSchema> DemandCsvImportSchema(CsvImportSchemaId id, CancellationToken cancellationToken)
        {
            return await dbContext.CsvImportSchemas
                .Where(r => r.Id == id.Value)
                .Select(r => r.ToCsvImportSchema())
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<CsvImportSchemaInfo>> GetCsvImportSchemas(CancellationToken cancellationToken)
        {
            return await dbContext.CsvImportSchemas
                .OrderBy(r => r.Name)
                .Select(r => r.ToCsvImportSchemaInfo())
                .ToListAsync(cancellationToken);
        }

        public async Task<CsvImportSchemaInfo> SaveImportMap(CsvImportSchema csvImportSchema, CancellationToken cancellationToken)
        {
            var data = await dbContext.CsvImportSchemas
                .SingleOrDefaultAsync(r => r.Id == csvImportSchema.Id.Value, cancellationToken);

            if (data == null)
                data = CreateCsvImportSchema(csvImportSchema);
            else
                UpdateCsvImportSchema(csvImportSchema, data);

            await dbContext.SaveChangesAsync(cancellationToken);

            return data.ToCsvImportSchemaInfo();
        }

        private CsvImportSchemaData CreateCsvImportSchema(CsvImportSchema csvImportSchema)
        {
            var data = new CsvImportSchemaData
            {
                Id = csvImportSchema.Id.Value,
                Name = csvImportSchema.Name,
                Mappings = csvImportSchema.Mappings
                    .Select(m => new CsvImportSchemaMappingData
                    { 
                        ImportSchemaId = csvImportSchema.Id.Value,
                        MappedProperty = m.MappedProperty,
                        ColumnIndex = m.ColumnIndex
                    }).ToList(),
            };

            dbContext.CsvImportSchemas.Add(data);

            return data;
        }

        private void UpdateCsvImportSchema(CsvImportSchema csvImportSchema, CsvImportSchemaData data)
        {
            data.Name = csvImportSchema.Name;

            UpdateMappings(data, csvImportSchema);
        }

        private void UpdateMappings(CsvImportSchemaData csvImportSchemaData, CsvImportSchema csvImportSchema)
        {
            foreach(var mapping in csvImportSchema.Mappings)
            {
                var existingMapping = csvImportSchemaData.Mappings
                    .Where(m => m.MappedProperty == mapping.MappedProperty)
                    .FirstOrDefault();

                if(existingMapping == null)
                {
                    csvImportSchemaData.Mappings.Add(new CsvImportSchemaMappingData
                    {
                        ImportSchemaId = csvImportSchemaData.Id,
                        MappedProperty = mapping.MappedProperty,
                        ColumnIndex = mapping.ColumnIndex
                    });
                }
                else
                {
                    existingMapping.ColumnIndex = mapping.ColumnIndex;
                }
            }

            var mappingsToRemove = csvImportSchemaData.Mappings
                .Where(m1 => !csvImportSchema.Mappings.Any(m2 => m1.MappedProperty == m2.MappedProperty))
                .ToList();

            foreach (var mappingToRemove in mappingsToRemove)
                csvImportSchemaData.Mappings.Remove(mappingToRemove);
        }
    }
}
