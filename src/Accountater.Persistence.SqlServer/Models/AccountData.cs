using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record AccountData
    {
        public const string TableName = "Account";

        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public List<FinancialTransactionData> Transactions { get; set; } = new();
    }
}
