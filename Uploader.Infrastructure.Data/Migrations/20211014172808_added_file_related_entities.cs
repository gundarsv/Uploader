using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class added_file_related_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploaderFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploaderFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploaderFiles_UploaderFileExtensions_ExtensionId",
                        column: x => x.ExtensionId,
                        principalTable: "UploaderFileExtensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UploaderFileComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploaderFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploaderFileComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploaderFileComments_UploaderFiles_UploaderFileId",
                        column: x => x.UploaderFileId,
                        principalTable: "UploaderFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploaderFileComments_UploaderFileId",
                table: "UploaderFileComments",
                column: "UploaderFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UploaderFiles_ExtensionId",
                table: "UploaderFiles",
                column: "ExtensionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploaderFileComments");

            migrationBuilder.DropTable(
                name: "UploaderFiles");
        }
    }
}
