using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class set_id_as_enabled_uploader_settings_pk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledUploaderSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnabledUploaderSettings",
                columns: table => new
                {
                    EnabledSettingsId = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
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
    }
}
