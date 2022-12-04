using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class edited_prodcutModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Product",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Product",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID");
        }
    }
}
