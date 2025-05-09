using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace matjerZaid.Migrations
{
    /// <inheritdoc />
    public partial class createInventorymovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventorymovements",
                columns: table => new
                {
                    movementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    movementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventorymovements", x => x.movementId);
                    table.ForeignKey(
                        name: "FK_inventorymovement_AspNetUsers_UserId",
                        column: x => x.createdBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventorymovement_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventorymovements_createdBy",
                table: "inventorymovements",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_inventorymovements_productId",
                table: "inventorymovements",
                column: "productId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventorymovements");
        }
    }
}
