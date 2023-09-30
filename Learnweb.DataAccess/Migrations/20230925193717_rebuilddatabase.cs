using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnweb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class rebuilddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Action");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { 2, "SciFi" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { 3, "History" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Ahmed");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { 5, "Mohamed" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { 9, "Saeed" });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[] { 4, 13, "Sameh" });
        }
    }
}
