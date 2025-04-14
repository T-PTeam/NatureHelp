using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeficiencyRelationshipsCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_CreatedBy",
                table: "WaterDeficiencies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_CreatedBy",
                table: "SoilDeficiencies",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilDeficiencies_Users_CreatedBy",
                table: "SoilDeficiencies",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDeficiencies_Users_CreatedBy",
                table: "WaterDeficiencies",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilDeficiencies_Users_CreatedBy",
                table: "SoilDeficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterDeficiencies_Users_CreatedBy",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_WaterDeficiencies_CreatedBy",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_SoilDeficiencies_CreatedBy",
                table: "SoilDeficiencies");
        }
    }
}
