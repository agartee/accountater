using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "Accountater",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Accountater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CategoryId",
                schema: "Accountater",
                table: "Transaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                schema: "Accountater",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                schema: "Accountater",
                table: "Transaction",
                column: "CategoryId",
                principalSchema: "Accountater",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                schema: "Accountater",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Accountater");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CategoryId",
                schema: "Accountater",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Accountater",
                table: "Transaction");
        }
    }
}
