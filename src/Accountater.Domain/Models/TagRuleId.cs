namespace Accountater.Domain.Models
{
    public readonly record struct TagRuleId(Guid Value)
    {
        public override string ToString() => Value.ToString();


        public static TagRuleId NewId()
        {
            return new TagRuleId(Guid.NewGuid());
        }
    }
}
