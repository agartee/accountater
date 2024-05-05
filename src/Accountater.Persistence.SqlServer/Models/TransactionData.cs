using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record TransactionData
    {
        public const string TableName = "Transaction";

        public required Guid Id { get; init; }
        public required Guid AccountId { get; init; }
        public required DateTime TransactionDate { get; init; }
        public required string Description { get; init; }

        [Column(TypeName = "decimal(10,2)")]
        public required decimal Amount { get; init; }

        public AccountData? Account { get; set; }
        public List<TagData> Tags { get; set; } = new();
    }
}
