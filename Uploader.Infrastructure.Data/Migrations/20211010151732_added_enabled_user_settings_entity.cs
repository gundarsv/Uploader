using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class added_enabled_user_settings_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UploaderSettings_IsEnabled",
                table: "UploaderSettings");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "UploaderSettings");

            migrationBuilder.CreateTable(
                name: "EnabledUserSettings",
                columns: table => new
                {
                    EnabledSettingsId = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnabledUserSettings", x => x.EnabledSettingsId);
                    table.ForeignKey(
                        name: "FK_EnabledUserSettings_UploaderSettings_EnabledSettingsId",
                        column: x => x.EnabledSettingsId,
                        principalTable: "UploaderSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledUserSettings");

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
    }
}
