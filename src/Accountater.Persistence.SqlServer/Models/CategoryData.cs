using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record CategoryData
    {
        public const string TableName = "Category";

        public required Guid Id { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; }

        public List<FinancialTransactionData> Transactions { get; set; } = new();
    }
}
