using Accountater.Domain.Models;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionCsvParser
    {
        Task<IEnumerable<FinancialTransactionImport>> Parse(CsvImportSchemaId csvImportSchemaId, Stream stream, 
            CancellationToken cancellationToken);
    }

    public class FinancialTransactionCsvParser : IFinancialTransactionCsvParser
    {
        private readonly ICsvImportSchemaInfoReader csvImportSchemaInfoReader;

        public FinancialTransactionCsvParser(ICsvImportSchemaInfoReader csvImportSchemaInfoReader)
        {
            this.csvImportSchemaInfoReader = csvImportSchemaInfoReader;
        }

        public async Task<IEnumerable<FinancialTransactionImport>> Parse(CsvImportSchemaId csvImportSchemaId, Stream stream, CancellationToken cancellationToken)
        {
            var csvImportSchema = await csvImportSchemaInfoReader
                .DemandCsvImportSchemaInfo(csvImportSchemaId, cancellationToken);

            var csvParser = new CsvParser<TinyCsvFinancialTransaction>(
                new CsvParserOptions(skipHeader: true, fieldsSeparator: ','),
                new AccountaterFinancialTransactionCsvMapping(csvImportSchema));

            var result = csvParser
                .ReadFromStream(stream, Encoding.ASCII)
                .Select(r => new FinancialTransactionImport
                {
                    Date = r.Result.Date!.Value,
                    Description = r.Result.Description!,
                    Amount = r.Result.Amount!.Value
                })
                .ToList();

            return result;
        }

        #region TinyCsvParser Implementation

        private class AccountaterFinancialTransactionCsvMapping : CsvMapping<TinyCsvFinancialTransaction>
        {
            public AccountaterFinancialTransactionCsvMapping(CsvImportSchemaInfo csvImportSchema) : base()
            {
                var dateIndex = csvImportSchema.Mappings
                    .SingleOrDefault(m => m.MappedProperty == "Date")?.ColumnIndex
                    ?? throw new InvalidOperationException("Date mapping not found in import schema.");
                
                var descriptionIndex = csvImportSchema.Mappings
                    .SingleOrDefault(m => m.MappedProperty == "Description")?.ColumnIndex
                    ?? throw new InvalidOperationException("Description mapping not found in import schema.");
                
                var amountIndex = csvImportSchema.Mappings
                    .SingleOrDefault(m => m.MappedProperty == "Amount")?.ColumnIndex
                    ?? throw new InvalidOperationException("Amount mapping not found in import schema.");

                MapProperty(dateIndex, x => x.Date);
                MapProperty(descriptionIndex, x => x.Description);
                MapProperty(amountIndex, x => x.Amount);
            }
        }

        // note: defined to satisfy TinyCsvParser's "new()" constructor condition on the mapped type, and
        // does not require that we pollute our domain model with nullable properties
        private record TinyCsvFinancialTransaction
        {
            public DateTime? Date { get; init; }
            public string? Description { get; init; }
            public decimal? Amount { get; init; }
        }

        #endregion
    }
}
