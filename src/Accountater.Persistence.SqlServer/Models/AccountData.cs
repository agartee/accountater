using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record AccountData
    {
        public const string TableName = "Account";

        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }

        public List<TransactionData> Transactions { get; set; } = new();
    }
}
