using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class TagsExtended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "Accountater",
                table: "Tag",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Order",
                schema: "Accountater",
                table: "Tag",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "Accountater",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "Accountater",
                table: "Tag");
        }
    }
}
