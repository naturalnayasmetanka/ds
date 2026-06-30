using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FS.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class add_actual_media_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "actual_media_data",
                schema: "files",
                table: "media_assets",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_media_data",
                schema: "files",
                table: "media_assets");
        }
    }
}
