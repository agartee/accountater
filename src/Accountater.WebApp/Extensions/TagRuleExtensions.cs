using Accountater.Domain.Models;
using Accountater.WebApp.Models;

namespace Accountater.WebApp.Extensions
{
    public static class TagRuleExtensions
    {
        public static EditTagRuleViewModel ToTagRuleViewModel(this TagRuleInfo model)
        {
            return new EditTagRuleViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Expression = model.Expression,
                Tag = model.Tag,
            };
        }
    }
}
