using Accountater.Domain.Models;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Accountater.WebApp.Services
{
    public interface ICreditTransactionCsvParser
    {
        IEnumerable<CreditTransaction> Parse(Stream stream);
    }

    public class CreditTransactionCsvParser : ICreditTransactionCsvParser
    {
        public record CsvCreditTransaction
        {
            public string? OriginalAccountNumber { get; init; }
            public string? AccountNumber { get; init; }
            public DateTime? Date { get; init; }
            public DateTime? PostingDate { get; init; }
            public decimal? Amount { get; init; }
            public string? Merchant { get; init; }
            public string? MerchantCity { get; init; }
            public string? MerchantZip { get; init; }
            public string? ReferenceNumber { get; init; }
        }

        private class CreditTransactionCsvMapping : CsvMapping<CsvCreditTransaction>
        {
            public CreditTransactionCsvMapping() : base()
            {
                MapProperty(0, x => x.OriginalAccountNumber);
                MapProperty(1, x => x.AccountNumber);
                MapProperty(2, x => x.Date);
                MapProperty(3, x => x.PostingDate);
                MapProperty(4, x => x.Amount);
                MapProperty(5, x => x.Merchant);
                MapProperty(6, x => x.MerchantCity);
                MapProperty(7, x => x.MerchantZip);
                MapProperty(8, x => x.ReferenceNumber);
            }
        }

        public IEnumerable<CreditTransaction> Parse(Stream stream)
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new CreditTransactionCsvMapping();
            var csvParser = new CsvParser<CsvCreditTransaction>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromStream(stream, Encoding.ASCII)
                .Select(r => new CreditTransaction
                {
                    OriginalAccountNumber = r.Result.OriginalAccountNumber!,
                    AccountName = r.Result.AccountNumber!,
                    Date = r.Result.Date!.Value,
                    PostingDate = r.Result.PostingDate!.Value,
                    Amount = r.Result.Amount!.Value,
                    Merchant = r.Result.Merchant!,
                    MerchantCity = r.Result.MerchantCity!,
                    MerchantZip = r.Result.MerchantZip!,
                    ReferenceNumber = r.Result.ReferenceNumber!,

                })
                .ToList();

            return result;
        }
    }
}
