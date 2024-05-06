using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accountater.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddTagRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagRule_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Value",
                table: "Tag",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagRule_Name",
                table: "TagRule",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagRule_TagId",
                table: "TagRule",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagRule");

            migrationBuilder.DropIndex(
                name: "IX_Tag_Value",
                table: "Tag");
        }
    }
}
