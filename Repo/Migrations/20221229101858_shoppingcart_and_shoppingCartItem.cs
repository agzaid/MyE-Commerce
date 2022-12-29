using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class shoppingcart_and_shoppingCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingCart_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qauantity = table.Column<int>(type: "int", nullable: false),
                    ProductItemID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_Product_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartShoppingCartItem",
                columns: table => new
                {
                    ShoppingCartItemsID = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartUsersID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartShoppingCartItem", x => new { x.ShoppingCartItemsID, x.ShoppingCartUsersID });
                    table.ForeignKey(
                        name: "FK_ShoppingCartShoppingCartItem_ShoppingCart_ShoppingCartUsersID",
                        column: x => x.ShoppingCartUsersID,
                        principalTable: "ShoppingCart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartShoppingCartItem_ShoppingCartItem_ShoppingCartItemsID",
                        column: x => x.ShoppingCartItemsID,
                        principalTable: "ShoppingCartItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_AppUserId",
                table: "ShoppingCart",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ProductItemID",
                table: "ShoppingCartItem",
                column: "ProductItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartUsersID",
                table: "ShoppingCartShoppingCartItem",
                column: "ShoppingCartUsersID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "ShoppingCartItem");
        }
    }
}
