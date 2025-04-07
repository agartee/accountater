namespace Accountater.Domain.Models
{
    public record Tag
    {
        public required TagId Id { get; init; }
        public required string Value { get; set; }
        public string? Color { get; set; }
        public decimal? Order { get; set; }
    }
}
