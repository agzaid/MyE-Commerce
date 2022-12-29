using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class ShoppingCartRelationTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartShoppingCartItem");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ShoppingCartId",
                table: "ShoppingCartItem",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_ShoppingCart_ShoppingCartId",
                table: "ShoppingCartItem",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_ShoppingCart_ShoppingCartId",
                table: "ShoppingCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItem_ShoppingCartId",
                table: "ShoppingCartItem");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartItem");

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
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartUsersID",
                table: "ShoppingCartShoppingCartItem",
                column: "ShoppingCartUsersID");
        }
    }
}
