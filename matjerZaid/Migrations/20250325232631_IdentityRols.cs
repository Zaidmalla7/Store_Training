using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECApp.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9A2E45B8-6D7A-4D99-8A4F-B1E2A3F7E9D1", null, "admin", "ADMIN" },
                    { "A57C1B60-5D2B-4E23-9C1A-FAE928D76C23", null, "client", "CLIENT" },
                    { "B83D85C9-7E89-4D45-BB98-4A1D7B96C123", null, "seller", "SELLER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9A2E45B8-6D7A-4D99-8A4F-B1E2A3F7E9D1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "A57C1B60-5D2B-4E23-9C1A-FAE928D76C23");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "B83D85C9-7E89-4D45-BB98-4A1D7B96C123");
        }
    }
}
