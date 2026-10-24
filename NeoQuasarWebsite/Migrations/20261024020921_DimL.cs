using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoQuasarWebsite.Migrations
{
    /// <inheritdoc />
    public partial class DimL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DimensionLOCs",
                columns: table => new
                {
                    LOCCode = table.Column<string>(type: "TEXT", nullable: false),
                    DimensionID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionLOCs", x => x.LOCCode);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DimensionLOCs");
        }
    }
}
