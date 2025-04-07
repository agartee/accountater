namespace Accountater.Domain.Models
{
    public readonly record struct TagId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static TagId NewId()
        {
            return new TagId(Guid.NewGuid());
        }
    }
}
