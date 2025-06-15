using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocationBinding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laboratories_OrgLocations_LocationId",
                table: "Laboratories");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrgLocations_LocationId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilDeficiencies_NatLocations_LocationId",
                table: "SoilDeficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_OrgLocations_AddressId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterDeficiencies_NatLocations_LocationId",
                table: "WaterDeficiencies");

            migrationBuilder.DropTable(
                name: "NatLocations");

            migrationBuilder.DropTable(
                name: "OrgLocations");

            migrationBuilder.DropIndex(
                name: "IX_WaterDeficiencies_LocationId",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SoilDeficiencies_LocationId",
                table: "SoilDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_LocationId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Laboratories_LocationId",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Laboratories");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "WaterDeficiencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "WaterDeficiencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "SoilDeficiencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "SoilDeficiencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AllowedMembersCount",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Laboratories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Laboratories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "AllowedMembersCount",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Laboratories");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Laboratories");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "WaterDeficiencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "SoilDeficiencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Organizations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Laboratories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "NatLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RadiusAffected = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NatLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrgLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_LocationId",
                table: "WaterDeficiencies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_LocationId",
                table: "SoilDeficiencies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LocationId",
                table: "Organizations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_LocationId",
                table: "Laboratories",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Laboratories_OrgLocations_LocationId",
                table: "Laboratories",
                column: "LocationId",
                principalTable: "OrgLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrgLocations_LocationId",
                table: "Organizations",
                column: "LocationId",
                principalTable: "OrgLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilDeficiencies_NatLocations_LocationId",
                table: "SoilDeficiencies",
                column: "LocationId",
                principalTable: "NatLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrgLocations_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "OrgLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDeficiencies_NatLocations_LocationId",
                table: "WaterDeficiencies",
                column: "LocationId",
                principalTable: "NatLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
