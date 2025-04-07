using System.ComponentModel;

namespace Accountater.Domain.Models
{
    public enum AccountType
    {
        [Description("Bank")]
        Bank = 1,
        [Description("Credit Card")]
        CreditCard = 2
    }
}
