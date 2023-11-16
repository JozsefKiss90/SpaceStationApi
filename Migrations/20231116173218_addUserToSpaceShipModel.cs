using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class addUserToSpaceShipModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpaceStationId",
                table: "UserEntity",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpaceStationId",
                table: "UserEntity");
        }
    }
}
