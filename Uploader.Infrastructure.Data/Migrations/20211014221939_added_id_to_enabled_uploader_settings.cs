using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class added_id_to_enabled_uploader_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploaderFiles_UploaderFileExtensions_ExtensionId",
                table: "UploaderFiles");

            migrationBuilder.AlterColumn<int>(
                name: "ExtensionId",
                table: "UploaderFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EnabledUploaderSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UploaderFiles_UploaderFileExtensions_ExtensionId",
                table: "UploaderFiles",
                column: "ExtensionId",
                principalTable: "UploaderFileExtensions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploaderFiles_UploaderFileExtensions_ExtensionId",
                table: "UploaderFiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EnabledUploaderSettings");

            migrationBuilder.AlterColumn<int>(
                name: "ExtensionId",
                table: "UploaderFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UploaderFiles_UploaderFileExtensions_ExtensionId",
                table: "UploaderFiles",
                column: "ExtensionId",
                principalTable: "UploaderFileExtensions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
