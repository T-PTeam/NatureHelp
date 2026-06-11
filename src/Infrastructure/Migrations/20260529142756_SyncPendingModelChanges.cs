using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDailyVisits_UserId_VisitDate",
                table: "UserDailyVisits");

            migrationBuilder.DropIndex(
                name: "IX_DeficiencyConfirmations_UserId_DeficiencyId_DeficiencyType",
                table: "DeficiencyConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_Code",
                table: "Achievements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dict",
                table: "dict");

            migrationBuilder.DropIndex(
                name: "IX_dict_key",
                table: "dict");

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-4000-8000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "SoilDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "WaterDeficiencies",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11112222-3333-4444-5555-666677778888"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11223344-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"));

            migrationBuilder.DeleteData(
                table: "Laboratories",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Laboratories",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Laboratories",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.RenameTable(
                name: "dict",
                newName: "DictEntries");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "DictEntries",
                newName: "ValueJson");

            migrationBuilder.RenameColumn(
                name: "key",
                table: "DictEntries",
                newName: "EntryKey");

            migrationBuilder.AlterColumn<string>(
                name: "EntryKey",
                table: "DictEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DictEntries",
                table: "DictEntries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyVisits_UserId",
                table: "UserDailyVisits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeficiencyConfirmations_UserId",
                table: "DeficiencyConfirmations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDailyVisits_UserId",
                table: "UserDailyVisits");

            migrationBuilder.DropIndex(
                name: "IX_DeficiencyConfirmations_UserId",
                table: "DeficiencyConfirmations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DictEntries",
                table: "DictEntries");

            migrationBuilder.RenameTable(
                name: "DictEntries",
                newName: "dict");

            migrationBuilder.RenameColumn(
                name: "ValueJson",
                table: "dict",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "EntryKey",
                table: "dict",
                newName: "key");

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "dict",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dict",
                table: "dict",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "Code", "Description", "Icon", "IsActive", "RuleType", "SortOrder", "TargetInt", "Title" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-4000-8000-000000000001"), "first_report", "Create your first deficiency report", "eco", true, 0, 1, 1, "First report" },
                    { new Guid("a0000001-0000-4000-8000-000000000002"), "water_5", "Create 5 water deficiency reports", "water_drop", true, 1, 2, 5, "Water guardian" },
                    { new Guid("a0000001-0000-4000-8000-000000000003"), "soil_3", "Create 3 soil deficiency reports", "grass", true, 2, 3, 3, "Soil scientist" },
                    { new Guid("a0000001-0000-4000-8000-000000000004"), "xp_500", "Reach 500 total XP", "stars", true, 3, 4, 500, "Dedicated contributor" },
                    { new Guid("a0000001-0000-4000-8000-000000000005"), "level_3", "Reach level 3", "trending_up", true, 4, 5, 3, "Rising star" },
                    { new Guid("a0000001-0000-4000-8000-000000000006"), "confirm_5_others", "Confirm 5 deficiencies created by others", "verified", true, 5, 6, 5, "Community voice" },
                    { new Guid("a0000001-0000-4000-8000-000000000007"), "five_confirmations_own", "Have one of your reports confirmed by 5 people", "groups", true, 6, 7, 1, "Trusted report" }
                });

            migrationBuilder.InsertData(
                table: "Laboratories",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedOn", "IsPublic", "Latitude", "Longitude", "Title" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8668), false, 0.0, 0.0, "Biomedical Research Lab" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9007), false, 0.0, 0.0, "AI and Machine Learning Lab" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), null, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9011), false, 0.0, 0.0, "Genetics and Biotechnology Lab" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "AllowedMembersCount", "CreatedBy", "CreatedOn", "Title" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 11, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8020), "Global Research Institute" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 11, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8414), "International Tech Hub" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AchievementAlertsEnabled", "CreatedBy", "CreatedOn", "CurrentLevel", "DateOfBirth", "Email", "EmailConfirmationToken", "EmailNotificationsEnabled", "FirstName", "IsEmailConfirmed", "LaboratoryId", "LastName", "NewsletterEnabled", "OrganizationId", "PasswordHash", "PasswordResetToken", "PasswordResetTokenExpiry", "PhoneNumber", "ProfileIsPublic", "RefreshToken", "RefreshTokenExpireTime", "Role", "TotalXp" },
                values: new object[,]
                {
                    { new Guid("11112222-3333-4444-5555-666677778888"), true, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9649), 1, new DateTime(1985, 5, 19, 21, 0, 0, 0, DateTimeKind.Utc), "valentyn@example.com", null, true, "Valentyn", true, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Riabinchak", false, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "AQAAAAIAAYagAAAAECguO79y3aAyVPpzpWncaB4IYu9PYjpnVFccaS8craV/lS2/wsFIdGgP3zt57jcgng==", null, null, "+380501234567", true, null, null, 3, 0 },
                    { new Guid("11223344-5566-7788-99aa-bbccddeeff00"), true, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 585, DateTimeKind.Utc).AddTicks(5636), 1, new DateTime(1980, 3, 9, 22, 0, 0, 0, DateTimeKind.Utc), "igor@example.com", null, true, "Igor", true, new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Zaitsev", false, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "AQAAAAIAAYagAAAAEKxFyghqrxHSumgKLFEzw7dG6LzDHXmxeuQErcXaVxRD8l7pFWl/gJI94vUXdtBUHw==", null, null, "+49 17612345678", true, null, null, 3, 0 },
                    { new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), true, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 536, DateTimeKind.Utc).AddTicks(4675), 1, new DateTime(1990, 7, 14, 21, 0, 0, 0, DateTimeKind.Utc), "igorzayets@example.com", null, true, "Valentyn", true, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Riabinchak", false, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "AQAAAAIAAYagAAAAEAvDOvE1RJIgnTiRC1b1t8ovIg71oxhDmkd+tdUk85PBDMsoLY1lk5hiNFi2OI54yw==", null, null, "+380631234567", true, null, null, 3, 0 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(5064), "Genetic research data goes here...", new Guid("11112222-3333-4444-5555-666677778888"), "Annual Genetic Study", 1 },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6146), "Performance analysis data goes here...", new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "AI Algorithm Performance", 1 },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6152), "Pandemic analysis data goes here...", new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Global Pandemic Analysis", 0 }
                });

            migrationBuilder.InsertData(
                table: "SoilDeficiencies",
                columns: new[] { "Id", "Address", "AnalysisDate", "CadmiumConcentration", "ChangedModelLogEntityId", "CreatedBy", "CreatedOn", "DeficiencyMonitoringId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "IsPublic", "Latitude", "LeadConcentration", "Longitude", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "RadiusAffected", "ResponsibleUserId", "Title", "Type" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 1, 14, 22, 0, 0, 0, DateTimeKind.Utc), 1.2, null, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(599), null, "", 0, 0.69999999999999996, 120.0, true, 50.450099999999999, 150.0, 30.523399999999999, 0.59999999999999998, 3200.0, 45.0, 3.7999999999999998, 6.5, 0.80000000000000004, 10.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "First Soil def", 1 },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), null, new DateTime(2025, 1, 17, 22, 0, 0, 0, DateTimeKind.Utc), 2.5, null, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2791), null, "", 2, 0.90000000000000002, 200.0, true, 49.993499999999997, 250.0, 36.229199999999999, 1.1000000000000001, 1500.0, 60.0, 2.5, 5.9000000000000004, 1.5, 10.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Second Soil def", 1 },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), null, new DateTime(2025, 1, 19, 22, 0, 0, 0, DateTimeKind.Utc), 0.80000000000000004, null, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2808), null, "", 1, 0.5, 50.0, true, 48.464700000000001, 80.0, 35.0456, 0.29999999999999999, 4000.0, 30.0, 4.0999999999999996, 7.2000000000000002, 0.5, 10.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Soil def", 1 }
                });

            migrationBuilder.InsertData(
                table: "WaterDeficiencies",
                columns: new[] { "Id", "Address", "BiologicalOxygenDemand", "CadmiumConcentration", "ChangedModelLogEntityId", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "DeficiencyMonitoringId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "IsPublic", "Latitude", "LeadConcentration", "Longitude", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "RadiusAffected", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "Type" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), null, 4.5, 0.029999999999999999, null, 0.0, new Guid("11112222-3333-4444-5555-666677778888"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7718), null, "", 6.7999999999999998, 0, 1.2, true, 50.450099999999999, 0.14999999999999999, 30.523399999999999, 0.02, 0.0, 1500.0, 20.0, 7.2000000000000002, 0.10000000000000001, 2.1000000000000001, 0.0, 10.0, new Guid("11112222-3333-4444-5555-666677778888"), "First Water def", 500.0, 0 },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), null, 8.0, 0.14999999999999999, null, 0.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(253), null, "", 4.0, 2, 2.5, true, 49.8429, 0.5, 24.031600000000001, 0.10000000000000001, 0.0, 4000.0, 50.0, 6.5, 0.80000000000000004, 5.5, 0.0, 10.0, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "Second Water def", 800.0, 0 },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), null, 2.0, 0.01, null, 0.0, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(262), null, "", 7.5, 1, 0.90000000000000002, true, 46.482500000000002, 0.050000000000000003, 30.732600000000001, 0.0050000000000000001, 0.0, 800.0, 10.0, 8.0, 0.050000000000000003, 1.0, 0.0, 10.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Water def", 350.0, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyVisits_UserId_VisitDate",
                table: "UserDailyVisits",
                columns: new[] { "UserId", "VisitDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeficiencyConfirmations_UserId_DeficiencyId_DeficiencyType",
                table: "DeficiencyConfirmations",
                columns: new[] { "UserId", "DeficiencyId", "DeficiencyType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Code",
                table: "Achievements",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dict_key",
                table: "dict",
                column: "key",
                unique: true);
        }
    }
}
