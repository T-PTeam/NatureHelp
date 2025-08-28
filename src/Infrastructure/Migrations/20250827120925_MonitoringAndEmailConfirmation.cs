using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MonitoringAndEmailConfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeficiencyMonitoringId",
                table: "WaterDeficiencies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeficiencyMonitoringScheme_isMonitoringSoilDeficiencies",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeficiencyMonitoringScheme_isMonitoringWaterDeficiencies",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeficiencyMonitoringId",
                table: "SoilDeficiencies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeficiencyMonitoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsMonitoring = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeficiencyType = table.Column<int>(type: "integer", nullable: false),
                    DeficiencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeficiencyMonitoring", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeficiencyMonitoring_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_DeficiencyMonitoringId",
                table: "WaterDeficiencies",
                column: "DeficiencyMonitoringId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_DeficiencyMonitoringId",
                table: "SoilDeficiencies",
                column: "DeficiencyMonitoringId");

            migrationBuilder.CreateIndex(
                name: "IX_DeficiencyMonitoring_UserId",
                table: "DeficiencyMonitoring",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilDeficiencies_DeficiencyMonitoring_DeficiencyMonitoringId",
                table: "SoilDeficiencies",
                column: "DeficiencyMonitoringId",
                principalTable: "DeficiencyMonitoring",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDeficiencies_DeficiencyMonitoring_DeficiencyMonitoring~",
                table: "WaterDeficiencies",
                column: "DeficiencyMonitoringId",
                principalTable: "DeficiencyMonitoring",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilDeficiencies_DeficiencyMonitoring_DeficiencyMonitoringId",
                table: "SoilDeficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterDeficiencies_DeficiencyMonitoring_DeficiencyMonitoring~",
                table: "WaterDeficiencies");

            migrationBuilder.DropTable(
                name: "DeficiencyMonitoring");

            migrationBuilder.DropIndex(
                name: "IX_WaterDeficiencies_DeficiencyMonitoringId",
                table: "WaterDeficiencies");

            migrationBuilder.DropIndex(
                name: "IX_SoilDeficiencies_DeficiencyMonitoringId",
                table: "SoilDeficiencies");

            migrationBuilder.DropColumn(
                name: "DeficiencyMonitoringId",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "DeficiencyMonitoringScheme_isMonitoringSoilDeficiencies",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeficiencyMonitoringScheme_isMonitoringWaterDeficiencies",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeficiencyMonitoringId",
                table: "SoilDeficiencies");
        }
    }
}
