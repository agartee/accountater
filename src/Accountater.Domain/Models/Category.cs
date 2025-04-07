namespace Accountater.Domain.Models
{
    public record Category
    {
        public required CategoryId Id { get; init; }
        public required string Name { get; set; }
    }
}
