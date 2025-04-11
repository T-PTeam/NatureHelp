using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedModelLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChangedModelLogEntityId",
                table: "WaterDeficiencies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedModelLogEntityId",
                table: "SoilDeficiencies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChangedModelLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeficiencyType = table.Column<int>(type: "integer", nullable: false),
                    DeficiencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangesJson = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangedModelLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_ChangedModelLogEntityId",
                table: "WaterDeficiencies",
                column: "ChangedModelLogEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_ChangedModelLogEntityId",
                table: "SoilDeficiencies",
                column: "ChangedModelLogEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilDeficiencies_ChangedModelLogs_ChangedModelLogEntityId",
                table: "SoilDeficiencies",
                column: "ChangedModelLogEntityId",
                principalTable: "ChangedModelLogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDeficiencies_ChangedModelLogs_ChangedModelLogEntityId",
                table: "WaterDeficiencies",
                column: "ChangedModelLogEntityId",
                principalTable: "ChangedModelLogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilDeficiencies_ChangedModelLogs_ChangedModelLogEntityId",
                table: "SoilDeficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterDeficiencies_ChangedModelLogs_ChangedModelLogEntityId",
                table: "WaterDeficiencies");

            migrationBuilder.DropTable(
                name: "ChangedModelLogs");

            migrationBuilder.DropIndex(
                name: "IX_WaterDeficiencies_ChangedModelLogEntityId",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_SoilDeficiencies_ChangedModelLogEntityId",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "ChangedModelLogEntityId",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "ChangedModelLogEntityId",
                table: "SoilDeficiencies");
        }
    }
}
