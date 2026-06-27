using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FS.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class update_col_drop_duplicate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "asset_type1",
                table: "media_assets");

            migrationBuilder.AlterColumn<string>(
                name: "asset_type",
                table: "media_assets",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AssetType",
                table: "media_assets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "media_assets");

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
    }
}
