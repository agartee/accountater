using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class TransactionDateRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transaction",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Transaction",
                newName: "TransactionDate");
        }
    }
}
