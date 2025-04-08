using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public record TagForm
    {
        public required TagId Id { get; init; }
        public required string Value { get; set; }
        public required string Color { get; set; }
        public decimal Order { get; set; }
    }
}
