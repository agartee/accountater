namespace Accountater.Domain.Models
{
    public readonly record struct CategoryId(Guid Value)
    {
        public override string ToString() => Value.ToString();

        public static CategoryId NewId()
        {
            return new CategoryId(Guid.NewGuid());
        }

        public static CategoryId CreditCardPayment()
        {
            return new CategoryId(Guid.Parse("DDB64599-57AD-40D1-BCA1-FB006B55FE56"));
        }
    }
}
