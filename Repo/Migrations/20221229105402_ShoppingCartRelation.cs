using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class ShoppingCartRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductItemID",
                table: "ShoppingCartItem");

            migrationBuilder.RenameColumn(
                name: "ProductItemID",
                table: "ShoppingCartItem",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItem_ProductItemID",
                table: "ShoppingCartItem",
                newName: "IX_ShoppingCartItem_ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductID",
                table: "ShoppingCartItem",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductID",
                table: "ShoppingCartItem");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ShoppingCartItem",
                newName: "ProductItemID");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItem_ProductID",
                table: "ShoppingCartItem",
                newName: "IX_ShoppingCartItem_ProductItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductItemID",
                table: "ShoppingCartItem",
                column: "ProductItemID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
