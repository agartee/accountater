using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class TransactionMetadataUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionMetadataRule_Tag_TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropIndex(
                name: "IX_TransactionMetadataRule_TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropColumn(
                name: "TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.AddColumn<int>(
                name: "MetadataType",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MetadataValue",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMetadataRule_TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "TagDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionMetadataRule_Tag_TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "TagDataId",
                principalSchema: "Accountater",
                principalTable: "Tag",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionMetadataRule_Tag_TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropIndex(
                name: "IX_TransactionMetadataRule_TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropColumn(
                name: "MetadataType",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropColumn(
                name: "MetadataValue",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.DropColumn(
                name: "TagDataId",
                schema: "Accountater",
                table: "TransactionMetadataRule");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMetadataRule_TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionMetadataRule_Tag_TagId",
                schema: "Accountater",
                table: "TransactionMetadataRule",
                column: "TagId",
                principalSchema: "Accountater",
                principalTable: "Tag",
                principalColumn: "Id");
        }
    }
}
