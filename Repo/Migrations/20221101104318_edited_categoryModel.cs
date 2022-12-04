using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class edited_categoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSrc",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Category",
                newName: "ThumbnailImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailImage",
                table: "Category",
                newName: "ParentId");

            migrationBuilder.AddColumn<string>(
                name: "ImageSrc",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
