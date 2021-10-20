using Microsoft.EntityFrameworkCore.Migrations;

namespace Uploader.Infrastructure.Data.Migrations
{
    public partial class seed_uploader_database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UploaderFileExtensions",
                columns: new[] { "Id", "ConcurrencyStamp", "FileExtension" },
                values: new object[] { 1, "4c6b6cbb-50a7-42c4-bb81-b5e2ec0647ee", "jpg" });

            migrationBuilder.InsertData(
                table: "UploaderFileExtensions",
                columns: new[] { "Id", "ConcurrencyStamp", "FileExtension" },
                values: new object[] { 2, "8a9b8fcd-2ce0-4777-9a9b-22816a481296", "png" });

            migrationBuilder.InsertData(
                table: "UploaderSettings",
                columns: new[] { "Id", "ConcurrencyStamp", "MaxFileSize", "MaxHeight", "MaxWidth", "MinHeight", "MinWidth" },
                values: new object[] { 1, "89662660-274c-48e4-bcae-b49f2ad37792", 50L, 2000, 2000, 200, 200 });

            migrationBuilder.InsertData(
                table: "UploaderFileExtensionUploaderSettings",
                columns: new[] { "AllowedFileExtensionsId", "UploaderSettingsId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "UploaderFileExtensionUploaderSettings",
                columns: new[] { "AllowedFileExtensionsId", "UploaderSettingsId" },
                values: new object[] { 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UploaderFileExtensionUploaderSettings",
                keyColumn: "AllowedFileExtensionsId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UploaderFileExtensionUploaderSettings",
                keyColumn: "AllowedFileExtensionsId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UploaderFileExtensions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UploaderFileExtensions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UploaderSettings",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
