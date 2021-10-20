using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class updated_uploader_settings_with_isEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "UploaderSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploaderSettings_IsEnabled",
                table: "UploaderSettings",
                column: "IsEnabled",
                unique: true,
                filter: "[IsEnabled] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UploaderSettings_IsEnabled",
                table: "UploaderSettings");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "UploaderSettings");
        }
    }
}
