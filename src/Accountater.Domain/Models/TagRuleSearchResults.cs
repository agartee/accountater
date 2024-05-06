namespace Accountater.Domain.Models
{
    public class TagRuleSearchResults : SearchResults
    {
        public required IEnumerable<TagRuleInfo> TagRules { get; init; }
    }
}
