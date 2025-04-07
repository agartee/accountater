namespace Accountater.Domain.Models
{
    public readonly record struct CategoryId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static CategoryId NewId()
        {
            return new CategoryId(Guid.NewGuid());
        }
    }
}
