using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoQuasarWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    ContentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileID = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.ContentID);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MIMEType = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
