using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class subitemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SkuMainItem");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SkuMainItem",
                newName: "Quantity");

            migrationBuilder.CreateTable(
                name: "SkuSubItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarCodeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ExiperyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SkuMainItemId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkuSubItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SkuSubItem_SkuMainItem_SkuMainItemId",
                        column: x => x.SkuMainItemId,
                        principalTable: "SkuMainItem",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkuSubItem_SkuMainItemId",
                table: "SkuSubItem",
                column: "SkuMainItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkuSubItem");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SkuMainItem",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SkuMainItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
