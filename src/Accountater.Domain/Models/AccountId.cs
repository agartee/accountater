namespace Accountater.Domain.Models
{
    public readonly record struct AccountId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static AccountId NewId()
        {
            return new AccountId(Guid.NewGuid());
        }
    }
}
