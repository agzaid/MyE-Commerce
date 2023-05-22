using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class barcode_to_barcodeByte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Invoice");

            migrationBuilder.AddColumn<byte[]>(
                name: "BarcodeByte",
                table: "Invoice",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarcodeByte",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
