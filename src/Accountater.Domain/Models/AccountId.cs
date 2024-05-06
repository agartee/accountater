namespace Accountater.Domain.Models
{
    public record AccountId : Id<Guid>
    {
        public AccountId(Guid value) : base(value)
        {
        }

        public static AccountId NewId()
        {
            return new AccountId(Guid.NewGuid());
        }
    }
}
