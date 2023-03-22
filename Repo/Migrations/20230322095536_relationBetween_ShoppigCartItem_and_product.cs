using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class relationBetween_ShoppigCartItem_and_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItem_ProductID",
                table: "ShoppingCartItem");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ProductID",
                table: "ShoppingCartItem",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItem_ProductID",
                table: "ShoppingCartItem");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ProductID",
                table: "ShoppingCartItem",
                column: "ProductID",
                unique: true);
        }
    }
}
