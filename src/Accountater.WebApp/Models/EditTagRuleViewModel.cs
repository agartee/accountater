using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public class EditTagRuleViewModel
    {
        public required TagRuleId Id { get; init; }
        public required string Name { get; set; }
        public required string Expression { get; set; }
        public required string Tag { get; set; }
    }
}
