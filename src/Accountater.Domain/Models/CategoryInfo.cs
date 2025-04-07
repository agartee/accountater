namespace Accountater.Domain.Models
{
    public record CategoryInfo
    {
        public required CategoryId Id { get; init; }
        public required string Name { get; init; }
    }
}
