using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AivaptDotNet.Migrations
{
    public partial class AddMinecraftModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mc_coordinates",
                columns: table => new
                {
                    CoordinatesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    X = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Y = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Z = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    SubmitterId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mc_coordinates", x => x.CoordinatesId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mc_location",
                columns: table => new
                {
                    LocationId = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LocationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mc_location", x => x.LocationId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "McCoordinatesMcLocation",
                columns: table => new
                {
                    LinkedMcCoordinatesCoordinatesId = table.Column<int>(type: "int", nullable: false),
                    LocationsLocationId = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McCoordinatesMcLocation", x => new { x.LinkedMcCoordinatesCoordinatesId, x.LocationsLocationId });
                    table.ForeignKey(
                        name: "FK_McCoordinatesMcLocation_mc_coordinates_LinkedMcCoordinatesCo~",
                        column: x => x.LinkedMcCoordinatesCoordinatesId,
                        principalTable: "mc_coordinates",
                        principalColumn: "CoordinatesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_McCoordinatesMcLocation_mc_location_LocationsLocationId",
                        column: x => x.LocationsLocationId,
                        principalTable: "mc_location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_McCoordinatesMcLocation_LocationsLocationId",
                table: "McCoordinatesMcLocation",
                column: "LocationsLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "McCoordinatesMcLocation");

            migrationBuilder.DropTable(
                name: "mc_coordinates");

            migrationBuilder.DropTable(
                name: "mc_location");
        }
    }
}
