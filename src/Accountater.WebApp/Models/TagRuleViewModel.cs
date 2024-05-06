using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public class TagRuleViewModel
    {
        public required TagRuleId Id { get; init; }
        public string? Name { get; set; }
        public string? Expression { get; set; }
        public string? Tag { get; set; }
    }
}
