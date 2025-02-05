using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectLocationAndDeficienciesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToxicityLevel",
                table: "WaterDeficiencies");

            migrationBuilder.DropColumn(
                name: "ToxicityLevel",
                table: "SoilDeficiencies");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "NatLocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "NatLocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-5555-5555-5555-555555555555"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b6666666-6666-6666-6666-666666666666"),
                columns: new[] { "Latitude", "Longtitude" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"),
                column: "EDangerState",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333333333333"),
                column: "EDangerState",
                value: 1);

            migrationBuilder.UpdateData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"),
                column: "EDangerState",
                value: 2);

            migrationBuilder.UpdateData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"),
                column: "EDangerState",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "NatLocations");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "NatLocations");

            migrationBuilder.AddColumn<string>(
                name: "ToxicityLevel",
                table: "WaterDeficiencies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToxicityLevel",
                table: "SoilDeficiencies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"),
                column: "ToxicityLevel",
                value: "Moderate");

            migrationBuilder.UpdateData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"),
                columns: new[] { "EDangerState", "ToxicityLevel" },
                values: new object[] { 0, "High" });

            migrationBuilder.UpdateData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333333333333"),
                columns: new[] { "EDangerState", "ToxicityLevel" },
                values: new object[] { 0, "Low" });

            migrationBuilder.UpdateData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"),
                column: "ToxicityLevel",
                value: "Moderate");

            migrationBuilder.UpdateData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"),
                columns: new[] { "EDangerState", "ToxicityLevel" },
                values: new object[] { 0, "High" });

            migrationBuilder.UpdateData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"),
                columns: new[] { "EDangerState", "ToxicityLevel" },
                values: new object[] { 0, "Low" });
        }
    }
}
