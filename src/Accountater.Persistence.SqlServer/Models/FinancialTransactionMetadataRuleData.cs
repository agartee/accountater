using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public class FinancialTransactionMetadataRuleData
    {
        public const string TableName = "TransactionMetadataRule";

        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required Guid TagId { get; set; }
        public required string Expression { get; set; }

        public TagData? Tag { get; set; }
    }
}
