using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class TagRuleRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagRule",
                schema: "Accountater");

            migrationBuilder.AlterColumn<decimal>(
                name: "Order",
                schema: "Accountater",
                table: "Tag",
                type: "decimal(8,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionMetadataRule",
                schema: "Accountater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionMetadataRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionMetadataRule_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Accountater",
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMetadataRule_Name",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMetadataRule_TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionMetadataRule",
                schema: "Accountater");

            migrationBuilder.AlterColumn<decimal>(
                name: "Order",
                schema: "Accountater",
                table: "Tag",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,4)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TagRule",
                schema: "Accountater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagRule_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Accountater",
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagRule_Name",
                schema: "Accountater",
                table: "TagRule",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagRule_TagId",
                schema: "Accountater",
                table: "TagRule",
                column: "TagId");
        }
    }
}
