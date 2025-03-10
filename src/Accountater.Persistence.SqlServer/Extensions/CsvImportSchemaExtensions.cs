using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class CsvImportSchemaExtensions
    {
        public static CsvImportSchema ToCsvImportSchema(this CsvImportSchemaData model)
        {
            return new CsvImportSchema
            {
                Id = new CsvImportSchemaId(model.Id),
                Name = model.Name,
                Mappings = model.Mappings
                    .Select(m => m.ToCsvImportSchemaMapping())
                    .ToList()
            };
        }

        public static CsvImportSchemaInfo ToCsvImportSchemaInfo(this CsvImportSchemaData model)
        {
            return new CsvImportSchemaInfo
            {
                Id = new CsvImportSchemaId(model.Id),
                Name = model.Name,
                Mappings = model.Mappings
                    .Select(m => m.ToCsvImportSchemaMappingInfo())
                    .ToList()
            };
        }

        private static CsvImportSchemaMapping ToCsvImportSchemaMapping(this CsvImportSchemaMappingData model)
        {
            return new CsvImportSchemaMapping
            {
                ColumnIndex = model.ColumnIndex,
                MappedProperty = model.MappedProperty,
            };
        }

        private static CsvImportSchemaMappingInfo ToCsvImportSchemaMappingInfo(this CsvImportSchemaMappingData model)
        {
            return new CsvImportSchemaMappingInfo
            {
                ColumnIndex = model.ColumnIndex,
                MappedProperty = model.MappedProperty,
            };
        }
    }
}
