using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FS.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class update_col_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "media_assets");

            migrationBuilder.RenameColumn(
                name: "MediaStatus",
                table: "media_assets",
                newName: "media_status");

            migrationBuilder.RenameIndex(
                name: "IX_media_assets_MediaStatus_created_at",
                table: "media_assets",
                newName: "IX_media_assets_media_status_created_at");

            migrationBuilder.AlterColumn<string>(
                name: "asset_type",
                table: "media_assets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "asset_type1",
                table: "media_assets",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "asset_type1",
                table: "media_assets");

            migrationBuilder.RenameColumn(
                name: "media_status",
                table: "media_assets",
                newName: "MediaStatus");

            migrationBuilder.RenameIndex(
                name: "IX_media_assets_media_status_created_at",
                table: "media_assets",
                newName: "IX_media_assets_MediaStatus_created_at");

            migrationBuilder.AlterColumn<string>(
                name: "asset_type",
                table: "media_assets",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                table: "media_assets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
