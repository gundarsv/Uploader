using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class changed_comments_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploaderFileComments");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "UploaderFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "UploaderFiles");

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
        }
    }
}
