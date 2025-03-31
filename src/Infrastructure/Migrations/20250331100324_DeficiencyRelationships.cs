using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeficiencyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilDeficiencies_Users_CreatorId",
                table: "SoilDeficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterDeficiencies_Users_CreatorId",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_WaterDeficiencies_CreatorId",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_SoilDeficiencies_CreatorId",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SoilDeficiencies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "WaterDeficiencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "SoilDeficiencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_CreatorId",
                table: "WaterDeficiencies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_CreatorId",
                table: "SoilDeficiencies",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilDeficiencies_Users_CreatorId",
                table: "SoilDeficiencies",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDeficiencies_Users_CreatorId",
                table: "WaterDeficiencies",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
