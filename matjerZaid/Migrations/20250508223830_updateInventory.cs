using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace matjerZaid.Migrations
{
    /// <inheritdoc />
    public partial class updateInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "createdAt",
                table: "inventory",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "minimumStock",
                table: "inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updatedBy",
                table: "inventory",
                type: "nvarchar(450)",
                nullable: false
               );

          

            migrationBuilder.CreateIndex(
                name: "IX_inventory_updatedBy",
                table: "inventory",
                column: "updatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_AspNetUsers_UserId",
                table: "inventory",
                column: "updatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_AspNetUsers_UserId",
                table: "inventory");

            
            migrationBuilder.DropIndex(
                name: "IX_inventory_updatedBy",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "minimumStock",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "note",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "updatedBy",
                table: "inventory");

            

           
        }
    }
}
