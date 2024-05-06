namespace Accountater.Domain.Models
{
    public record TagRuleId : Id<Guid>
    {
        public TagRuleId(Guid value) : base(value)
        {
        }

        public static TagRuleId NewId()
        {
            return new TagRuleId(Guid.NewGuid());
        }
    }
}
