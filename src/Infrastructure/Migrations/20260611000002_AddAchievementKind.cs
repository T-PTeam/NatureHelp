using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddAchievementKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Achievements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedOn", "Description", "Icon", "IsActive", "Kind", "RuleType", "SortOrder", "TargetInt", "Title" },
                values: new object[,]
                {
                    {
                        new Guid("b0000002-0000-4000-8000-000000000001"),
                        "challenge_10_reports",
                        Guid.Empty,
                        new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc),
                        "Submit 10 deficiency reports to complete this challenge.",
                        "flag",
                        true,
                        1,
                        0,
                        100,
                        10,
                        "First 10 Reports"
                    },
                    {
                        new Guid("b0000002-0000-4000-8000-000000000002"),
                        "challenge_5_water",
                        Guid.Empty,
                        new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc),
                        "Submit 5 water deficiency reports.",
                        "water",
                        true,
                        1,
                        1,
                        101,
                        5,
                        "Water Watcher"
                    },
                    {
                        new Guid("b0000002-0000-4000-8000-000000000003"),
                        "challenge_100_xp",
                        Guid.Empty,
                        new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc),
                        "Earn 100 total XP across all activities.",
                        "star",
                        true,
                        1,
                        3,
                        102,
                        100,
                        "XP Challenger"
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("b0000002-0000-4000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("b0000002-0000-4000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("b0000002-0000-4000-8000-000000000003"));

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Achievements");
        }
    }
}
