using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCsvImportSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accountater");

            migrationBuilder.RenameTable(
                name: "TransactionTag",
                newName: "TransactionTag",
                newSchema: "Accountater");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transaction",
                newSchema: "Accountater");

            migrationBuilder.RenameTable(
                name: "TagRule",
                newName: "TagRule",
                newSchema: "Accountater");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tag",
                newSchema: "Accountater");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Account",
                newSchema: "Accountater");

            migrationBuilder.CreateTable(
                name: "CsvImportSchema",
                schema: "Accountater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvImportSchema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CsvImportSchemaMapping",
                schema: "Accountater",
                columns: table => new
                {
                    ImportSchemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MappedProperty = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColumnIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvImportSchemaMapping", x => new { x.ImportSchemaId, x.MappedProperty });
                    table.ForeignKey(
                        name: "FK_CsvImportSchemaMapping_CsvImportSchema_ImportSchemaId",
                        column: x => x.ImportSchemaId,
                        principalSchema: "Accountater",
                        principalTable: "CsvImportSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CsvImportSchemaMapping",
                schema: "Accountater");

            migrationBuilder.DropTable(
                name: "CsvImportSchema",
                schema: "Accountater");

            migrationBuilder.RenameTable(
                name: "TransactionTag",
                schema: "Accountater",
                newName: "TransactionTag");

            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "Accountater",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "TagRule",
                schema: "Accountater",
                newName: "TagRule");

            migrationBuilder.RenameTable(
                name: "Tag",
                schema: "Accountater",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "Accountater",
                newName: "Account");
        }
    }
}
