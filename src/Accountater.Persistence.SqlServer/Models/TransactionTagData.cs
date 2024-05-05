using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record TransactionTagData
    {
        public const string TableName = "TransactionTag";

        public required Guid TransactionId { get; set; }
        public required Guid TagId { get; set; }

        public FinancialTransactionData? Transaction { get; set; }
        public TagData? Tag { get; set; }
    }
}
