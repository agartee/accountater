using Accountater.Domain.Models;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Accountater.WebApp.Services
{
    public interface ICheckingTransactionCsvParser
    {
        IEnumerable<CheckingTransaction> Parse(Stream stream);
    }

    public class CheckingTransactionCsvParser : ICheckingTransactionCsvParser
    {
        public record CvsCheckingTransaction
        {
            public DateTime? Date { get; init; }
            public string? Description { get; init; }
            public string? Category { get; init; }
            public decimal? Amount { get; init; }
        }

        private class CheckingTransactionCsvMapping : CsvMapping<CvsCheckingTransaction>
        {
            public CheckingTransactionCsvMapping() : base()
            {
                MapProperty(0, x => x.Date);
                MapProperty(1, x => x.Description);
                MapProperty(2, x => x.Category);
                MapProperty(3, x => x.Amount);
            }
        }

        public IEnumerable<CheckingTransaction> Parse(Stream stream)
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new CheckingTransactionCsvMapping();
            var csvParser = new CsvParser<CvsCheckingTransaction>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromStream(stream, Encoding.ASCII)
                .Select(r => new CheckingTransaction
                {
                    Date = r.Result.Date!.Value,
                    Description = r.Result.Description!,
                    Category = r.Result.Category!,
                    Amount = r.Result.Amount!.Value,
                })

                .ToList();

            return result;
        }
    }
}
