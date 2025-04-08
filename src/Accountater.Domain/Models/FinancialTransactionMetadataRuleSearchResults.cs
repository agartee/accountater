namespace Accountater.Domain.Models
{
    public class FinancialTransactionMetadataRuleSearchResults : SearchResults
    {
        public required IEnumerable<FinancialTransactionMetadataRuleInfo> Rules { get; init; }
    }
}
