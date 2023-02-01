using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AivaptDotNet.Migrations
{
    public partial class AddCoordinateDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "mc_coordinates",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "mc_coordinates");
        }
    }
}
