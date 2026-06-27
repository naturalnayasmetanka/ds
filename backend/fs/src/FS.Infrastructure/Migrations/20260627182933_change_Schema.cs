using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FS.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class change_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "files");

            migrationBuilder.RenameTable(
                name: "media_assets",
                newName: "media_assets",
                newSchema: "files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "media_assets",
                schema: "files",
                newName: "media_assets");
        }
    }
}
