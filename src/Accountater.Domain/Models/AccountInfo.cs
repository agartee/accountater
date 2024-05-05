namespace Accountater.Domain.Models
{
    public class AccountInfo
    {
        public required AccountId AccountId { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}
