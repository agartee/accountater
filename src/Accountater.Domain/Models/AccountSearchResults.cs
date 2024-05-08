namespace Accountater.Domain.Models
{
    public class AccountSearchResults : SearchResults
    {
        public required IEnumerable<AccountInfo> Accounts { get; init; }
    }
}
