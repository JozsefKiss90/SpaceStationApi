using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpaceShipAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "spacestation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StorageLevel = table.Column<int>(type: "integer", nullable: false),
                    HangarLevel = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spacestation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_spacestation_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "spaceship",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    EngineLevel = table.Column<int>(type: "integer", nullable: false),
                    ShieldLevel = table.Column<int>(type: "integer", nullable: false),
                    ShieldEnergy = table.Column<int>(type: "integer", nullable: false),
                    SpaceStationId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spaceship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_spaceship_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_spaceship_spacestation_SpaceStationId",
                        column: x => x.SpaceStationId,
                        principalTable: "spacestation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "minership",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DrillLevel = table.Column<int>(type: "integer", nullable: false),
                    StorageLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_minership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_minership_spaceship_Id",
                        column: x => x.Id,
                        principalTable: "spaceship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scoutship",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ScannerLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scoutship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scoutship_spaceship_Id",
                        column: x => x.Id,
                        principalTable: "spaceship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoredResources",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpaceStationId = table.Column<long>(type: "bigint", nullable: false),
                    MinerShipId = table.Column<long>(type: "bigint", nullable: false),
                    ResourceType = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredResources_minership_MinerShipId",
                        column: x => x.MinerShipId,
                        principalTable: "minership",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoredResources_spacestation_SpaceStationId",
                        column: x => x.SpaceStationId,
                        principalTable: "spacestation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_spaceship_SpaceStationId",
                table: "spaceship",
                column: "SpaceStationId");

            migrationBuilder.CreateIndex(
                name: "IX_spaceship_UserId",
                table: "spaceship",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_spacestation_UserId",
                table: "spacestation",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoredResources_MinerShipId",
                table: "StoredResources",
                column: "MinerShipId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredResources_SpaceStationId",
                table: "StoredResources",
                column: "SpaceStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scoutship");

            migrationBuilder.DropTable(
                name: "StoredResources");

            migrationBuilder.DropTable(
                name: "minership");

            migrationBuilder.DropTable(
                name: "spaceship");

            migrationBuilder.DropTable(
                name: "spacestation");

            migrationBuilder.DropTable(
                name: "UserEntity");
        }
    }
}
