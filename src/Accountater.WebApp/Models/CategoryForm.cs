using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public record CategoryForm
    {
        public required CategoryId Id { get; init; }
        public required string Name { get; set; }
    }
}
