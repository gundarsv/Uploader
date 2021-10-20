using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class added_enabled_uploader_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnabledUploaderSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnabledSettingsId = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnabledUploaderSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnabledUploaderSettings_UploaderSettings_EnabledSettingsId",
                        column: x => x.EnabledSettingsId,
                        principalTable: "UploaderSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnabledUploaderSettings_EnabledSettingsId",
                table: "EnabledUploaderSettings",
                column: "EnabledSettingsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledUploaderSettings");
        }
    }
}
