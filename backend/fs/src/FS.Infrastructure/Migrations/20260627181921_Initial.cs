using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FS.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "media_assets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetType = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    raw_key = table.Column<string>(type: "jsonb", nullable: false),
                    owner_context = table.Column<string>(type: "text", nullable: false),
                    owner_entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaStatus = table.Column<string>(type: "text", nullable: false),
                    asset_type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    media_data = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_assets", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_media_assets_MediaStatus_created_at",
                table: "media_assets",
                columns: new[] { "MediaStatus", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_assets");
        }
    }
}
