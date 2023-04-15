using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class skuMainItem_Edited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkuMainItem_BarCodeNumber",
                table: "SkuMainItem");

            migrationBuilder.DropColumn(
                name: "BarCodeNumber",
                table: "SkuMainItem");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "SkuMainItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SkuMainItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SkuMainItem",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkuMainItem_CategoryId",
                table: "SkuMainItem",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkuMainItem_Category_CategoryId",
                table: "SkuMainItem",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkuMainItem_Category_CategoryId",
                table: "SkuMainItem");

            migrationBuilder.DropIndex(
                name: "IX_SkuMainItem_CategoryId",
                table: "SkuMainItem");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SkuMainItem");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SkuMainItem");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "SkuMainItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BarCodeNumber",
                table: "SkuMainItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkuMainItem_BarCodeNumber",
                table: "SkuMainItem",
                column: "BarCodeNumber",
                unique: true,
                filter: "[BarCodeNumber] IS NOT NULL");
        }
    }
}
