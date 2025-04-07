namespace Accountater.Domain.Models
{
    public record TagInfo
    {
        public required TagId Id { get; init; }
        public required string Value { get; init; }
        public string? Color { get; init; }
        public decimal? Order { get; init; }
    }
}
