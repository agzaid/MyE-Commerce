using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Category",
               columns: table => new
               {
                   ID = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   DisplayOrder = table.Column<int>(type: "int", nullable: false),
                   ImageSrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                   Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Published = table.Column<bool>(type: "bit", nullable: false),
                   Status = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Category", x => x.ID);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
