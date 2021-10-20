using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class renamed_enabled_user_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledUserSettings");

            migrationBuilder.CreateTable(
                name: "EnabledUploaderSettings",
                columns: table => new
                {
                    EnabledSettingsId = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnabledUploaderSettings", x => x.EnabledSettingsId);
                    table.ForeignKey(
                        name: "FK_EnabledUploaderSettings_UploaderSettings_EnabledSettingsId",
                        column: x => x.EnabledSettingsId,
                        principalTable: "UploaderSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledUploaderSettings");

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
    }
}
