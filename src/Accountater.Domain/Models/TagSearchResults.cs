namespace Accountater.Domain.Models
{
    public class TagSearchResults : SearchResults
    {
        public required IEnumerable<TagInfo> Tags { get; init; }
    }
}
