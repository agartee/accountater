using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class TagRuleExtensions
    {
        public static TagRule ToTagRule(this TagRuleData model)
        {
            return new TagRule
            {
                Id = new TagRuleId(model.Id),
                Name = model.Name,
                Tag = model.Tag!.Value,
                Expression = model.Expression
            };
        }

        public static TagRuleInfo ToTagRuleInfo(this TagRuleData model)
        {
            return new TagRuleInfo
            {
                Id = new TagRuleId(model.Id),
                Name = model.Name,
                Tag = model.Tag!.Value,
                Expression = model.Expression
            };
        }
    }
}
