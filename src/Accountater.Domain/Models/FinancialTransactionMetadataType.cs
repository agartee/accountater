using System.ComponentModel;

namespace Accountater.Domain.Models
{
    public enum FinancialTransactionMetadataType
    {
        [Description("Not Specified")]
        NotSpecified = 0,
        [Description("Category")]
        Category = 1,
        [Description("Tag")]
        Tag = 2
    }
}
