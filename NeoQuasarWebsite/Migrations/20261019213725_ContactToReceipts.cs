using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoQuasarWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ContactToReceipts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "Receipts");
        }
    }
}
