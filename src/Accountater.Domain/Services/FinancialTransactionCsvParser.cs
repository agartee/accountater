using Accountater.Domain.Models;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionCsvParser
    {
        IEnumerable<FinancialTransactionImport> Parse(Stream stream);
    }

    public class FinancialTransactionCsvParser : IFinancialTransactionCsvParser
    {
        public record CvsFinancialTransaction
        {
            public DateTime? Date { get; init; }
            public string? Description { get; init; }
            public decimal? Amount { get; init; }
        }

        private class FinancialTransactionCsvMapping : CsvMapping<CvsFinancialTransaction>
        {
            public FinancialTransactionCsvMapping() : base()
            {
                MapProperty(0, x => x.Date);
                MapProperty(1, x => x.Description);
                MapProperty(3, x => x.Amount);
            }
        }

        public IEnumerable<FinancialTransactionImport> Parse(Stream stream)
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new FinancialTransactionCsvMapping();
            var csvParser = new CsvParser<CvsFinancialTransaction>(csvParserOptions, csvMapper);

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
    }
}
