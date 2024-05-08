using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public class EditAccountViewModel
    {
        public required AccountId Id { get; init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
