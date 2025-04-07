namespace Accountater.Domain.Models
{
    public class CategorySearchResults : SearchResults
    {
        public required IEnumerable<CategoryInfo> Categories { get; init; }
    }
}
