namespace Accountater.Domain
{
    public record CreditTransaction
    {
        public required string OriginalAccountNumber { get; init; }
        public required string AccountNumber { get; init; }
        public required DateTime TransactionDate { get; init; }
        public required DateTime PostingDate { get; init; }
        public required decimal Amount { get; init; }
        public required string Merchant { get; init; }
        public required string MerchantCity { get; init; }
        public required string MerchantZip { get; init; }
        public required string ReferenceNumber { get; init; }

    }
}
