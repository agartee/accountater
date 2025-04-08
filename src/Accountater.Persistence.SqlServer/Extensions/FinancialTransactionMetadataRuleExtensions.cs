using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class FinancialTransactionMetadataRuleExtensions
    {
        public static FinancialTransactionMetadataRule ToFinancialTransactionMetadataRule(this FinancialTransactionMetadataRuleData model)
        {
            return new FinancialTransactionMetadataRule
            {
                Id = new FinancialTransactionMetadataRuleId(model.Id),
                Name = model.Name,
                MetadataType = model.MetadataType,
                MetadataValue = model.MetadataValue,
                Expression = model.Expression
            };
        }

        public static FinancialTransactionMetadataRuleInfo ToFinancialTransactionMetadataRuleInfo(this FinancialTransactionMetadataRuleData model)
        {
            return new FinancialTransactionMetadataRuleInfo
            {
                Id = new FinancialTransactionMetadataRuleId(model.Id),
                Name = model.Name,
                MetadataType = model.MetadataType,
                MetadataValue = model.MetadataValue,
                Expression = model.Expression
            };
        }
    }
}
