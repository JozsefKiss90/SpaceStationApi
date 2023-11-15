using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForNullableSpaceStationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spaceship_spacestation_SpaceStationId",
                table: "spaceship");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredResources_spacestation_SpaceStationId",
                table: "StoredResources");

            migrationBuilder.AlterColumn<long>(
                name: "SpaceStationId",
                table: "spaceship",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_spaceship_spacestation_SpaceStationId",
                table: "spaceship",
                column: "SpaceStationId",
                principalTable: "spacestation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredResources_spacestation_SpaceStationId",
                table: "StoredResources",
                column: "SpaceStationId",
                principalTable: "spacestation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spaceship_spacestation_SpaceStationId",
                table: "spaceship");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredResources_spacestation_SpaceStationId",
                table: "StoredResources");

            migrationBuilder.AlterColumn<long>(
                name: "SpaceStationId",
                table: "spaceship",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_spaceship_spacestation_SpaceStationId",
                table: "spaceship",
                column: "SpaceStationId",
                principalTable: "spacestation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredResources_spacestation_SpaceStationId",
                table: "StoredResources",
                column: "SpaceStationId",
                principalTable: "spacestation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
