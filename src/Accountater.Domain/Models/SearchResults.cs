namespace Accountater.Domain.Models
{
    public abstract class SearchResults
    {
        public string? SearchText { get; init; }
        public required int PageSize { get; init; }
        public required int PageIndex { get; init; }
        public required int TotalCount { get; init; }
    }
}
