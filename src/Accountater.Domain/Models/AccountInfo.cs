namespace Accountater.Domain.Models
{
    public class AccountInfo
    {
        public required AccountId Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}
