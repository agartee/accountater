namespace Accountater.Domain.Models
{
    public class Account
    {
        public required AccountId Id { get; init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
