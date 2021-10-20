using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class initialize_uploader_database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploaderFileExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploaderFileExtensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploaderSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxFileSize = table.Column<long>(type: "bigint", nullable: false),
                    MaxHeight = table.Column<int>(type: "int", nullable: false),
                    MaxWidth = table.Column<int>(type: "int", nullable: false),
                    MinHeight = table.Column<int>(type: "int", nullable: false),
                    MinWidth = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploaderSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploaderFileExtensionUploaderSettings",
                columns: table => new
                {
                    AllowedFileExtensionsId = table.Column<int>(type: "int", nullable: false),
                    UploaderSettingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploaderFileExtensionUploaderSettings", x => new { x.AllowedFileExtensionsId, x.UploaderSettingsId });
                    table.ForeignKey(
                        name: "FK_UploaderFileExtensionUploaderSettings_UploaderFileExtensions_AllowedFileExtensionsId",
                        column: x => x.AllowedFileExtensionsId,
                        principalTable: "UploaderFileExtensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploaderFileExtensionUploaderSettings_UploaderSettings_UploaderSettingsId",
                        column: x => x.UploaderSettingsId,
                        principalTable: "UploaderSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploaderFileExtensionUploaderSettings_UploaderSettingsId",
                table: "UploaderFileExtensionUploaderSettings",
                column: "UploaderSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploaderFileExtensionUploaderSettings");

            migrationBuilder.DropTable(
                name: "UploaderFileExtensions");

            migrationBuilder.DropTable(
                name: "UploaderSettings");
        }
    }
}
