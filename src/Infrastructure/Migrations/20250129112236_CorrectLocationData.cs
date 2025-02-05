using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longtitude",
                table: "NatLocations",
                newName: "Longitude");

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 50.450099999999999, 30.523399999999999 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 49.8429, 24.031600000000001 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 46.482500000000002, 30.732600000000001 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 50.450099999999999, 30.523399999999999 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-5555-5555-5555-555555555555"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 49.993499999999997, 36.229199999999999 });

            migrationBuilder.UpdateData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b6666666-6666-6666-6666-666666666666"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 48.464700000000001, 35.0456 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "NatLocations",
                newName: "Longtitude");

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
        }
    }
}
