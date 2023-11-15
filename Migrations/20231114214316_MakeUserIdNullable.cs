using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spaceship_UserEntity_UserId",
                table: "spaceship");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "spaceship",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_spaceship_UserEntity_UserId",
                table: "spaceship",
                column: "UserId",
                principalTable: "UserEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spaceship_UserEntity_UserId",
                table: "spaceship");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "spaceship",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_spaceship_UserEntity_UserId",
                table: "spaceship",
                column: "UserId",
                principalTable: "UserEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
