namespace Accountater.Domain.Models
{
    public class TagRuleSearchCriteria
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }
}
