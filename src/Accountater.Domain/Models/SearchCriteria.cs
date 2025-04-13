namespace Accountater.Domain.Models
{
    public class SearchCriteria
    {
        public string? SearchText { get; init; }
        public bool IsExactMatch { get; init; } = false;
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }
}
