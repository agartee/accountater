using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record FinancialTransactionData
    {
        public const string TableName = "Transaction";

        public required Guid Id { get; init; }
        public required Guid AccountId { get; init; }
        public required DateTime Date { get; init; }
        public required string Description { get; init; }

        [Column(TypeName = "decimal(10,2)")]
        public required decimal Amount { get; init; }
        public Guid? CategoryId { get; set; }

        public AccountData? Account { get; set; }
        public CategoryData? Category { get; set; }
        public List<TagData> Tags { get; set; } = new();
    }
}
