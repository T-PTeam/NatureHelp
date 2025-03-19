using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Autogenerating_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NatLocations",
                columns: new[] { "Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected" },
                values: new object[,]
                {
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Addis Ababa", "Ethiopia", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6502), 50.450099999999999, 30.523399999999999, 10.0 },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Mumbai", "India", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7139), 49.8429, 24.031600000000001, 10.0 },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), "Phoenix", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7143), 46.482500000000002, 30.732600000000001, 10.0 },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), "Sydney", "Australia", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7145), 50.450099999999999, 30.523399999999999, 10.0 },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), "Beijing", "China", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7147), 49.993499999999997, 36.229199999999999, 10.0 },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), "Uzhhorod", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7160), 48.464700000000001, 35.0456, 10.0 }
                });

            migrationBuilder.InsertData(
                table: "OrgLocations",
                columns: new[] { "Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Kyiv", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(6609), "Shevchenkivsky", "Kyiv" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "New York", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7525), "Manhattan", "New York" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Berlin", "Germany", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7530), "Mitte", "Berlin" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Kyiv", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7534), "Kyiv City District", "Kyiv Oblast" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "New York", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7537), "Manhattan District", "New York State" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Berlin", "Germany", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7538), "Mitte District", "Berlin" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Rio de Janeiro", "Brazil", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7540), "Central District", "Rio de Janeiro State" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Paris", "France", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7541), "Paris City District", "Île-de-France" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Tokyo", "Japan", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7548), "Tokyo Metropolis District", "Kantō" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Rome", "Italy", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(7550), "Rome City District", "Lazio" }
                });

            migrationBuilder.InsertData(
                table: "Laboratories",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LocationId", "Title" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8668), new Guid("11111111-1111-1111-1111-111111111111"), "Biomedical Research Lab" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9007), new Guid("33333333-3333-3333-3333-333333333333"), "AI and Machine Learning Lab" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9011), new Guid("22222222-2222-2222-2222-222222222222"), "Genetics and Biotechnology Lab" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LocationId", "Title" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8020), new Guid("11111111-1111-1111-1111-111111111111"), "Global Research Institute" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8414), new Guid("33333333-3333-3333-3333-333333333333"), "International Tech Hub" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessToken", "AccessTokenExpireTime", "AddressId", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "LaboratoryId", "LastName", "OrganizationId", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime", "Role" },
                values: new object[,]
                {
                    { new Guid("11112222-3333-4444-5555-666677778888"), null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9649), new DateTime(1985, 5, 19, 21, 0, 0, 0, DateTimeKind.Utc), "valentyn@example.com", "Valentyn", new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Riabinchak", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "AQAAAAIAAYagAAAAECguO79y3aAyVPpzpWncaB4IYu9PYjpnVFccaS8craV/lS2/wsFIdGgP3zt57jcgng==", "+380501234567", null, null, 3 },
                    { new Guid("11223344-5566-7788-99aa-bbccddeeff00"), null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 585, DateTimeKind.Utc).AddTicks(5636), new DateTime(1980, 3, 9, 22, 0, 0, 0, DateTimeKind.Utc), "igor@example.com", "Igor", new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Zaitsev", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "AQAAAAIAAYagAAAAEKxFyghqrxHSumgKLFEzw7dG6LzDHXmxeuQErcXaVxRD8l7pFWl/gJI94vUXdtBUHw==", "+49 17612345678", null, null, 3 },
                    { new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 536, DateTimeKind.Utc).AddTicks(4675), new DateTime(1990, 7, 14, 21, 0, 0, 0, DateTimeKind.Utc), "igorzayets@example.com", "Valentyn", new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Riabinchak", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "AQAAAAIAAYagAAAAEAvDOvE1RJIgnTiRC1b1t8ovIg71oxhDmkd+tdUk85PBDMsoLY1lk5hiNFi2OI54yw==", "+380631234567", null, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(5064), "Genetic research data goes here...", new Guid("11112222-3333-4444-5555-666677778888"), "Annual Genetic Study", 1 },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6146), "Performance analysis data goes here...", new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "AI Algorithm Performance", 1 },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6152), "Pandemic analysis data goes here...", new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Global Pandemic Analysis", 0 }
                });

            migrationBuilder.InsertData(
                table: "SoilDeficiencies",
                columns: new[] { "Id", "AnalysisDate", "CadmiumConcentration", "CreatedBy", "CreatedOn", "CreatorId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "ResponsibleUserId", "Title", "Type" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 14, 22, 0, 0, 0, DateTimeKind.Utc), 1.2, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(599), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 0, 0.69999999999999996, 120.0, 150.0, new Guid("b4444444-4444-4444-4444-444444444444"), 0.59999999999999998, 3200.0, 45.0, 3.7999999999999998, 6.5, 0.80000000000000004, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "First Soil def", 0 },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 17, 22, 0, 0, 0, DateTimeKind.Utc), 2.5, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2791), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 2, 0.90000000000000002, 200.0, 250.0, new Guid("b5555555-5555-5555-5555-555555555555"), 1.1000000000000001, 1500.0, 60.0, 2.5, 5.9000000000000004, 1.5, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Second Soil def", 0 },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), new DateTime(2025, 1, 19, 22, 0, 0, 0, DateTimeKind.Utc), 0.80000000000000004, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2808), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 1, 0.5, 50.0, 80.0, new Guid("b6666666-6666-6666-6666-666666666666"), 0.29999999999999999, 4000.0, 30.0, 4.0999999999999996, 7.2000000000000002, 0.5, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Soil def", 0 }
                });

            migrationBuilder.InsertData(
                table: "WaterDeficiencies",
                columns: new[] { "Id", "BiologicalOxygenDemand", "CadmiumConcentration", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "CreatorId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "Type" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), 4.5, 0.029999999999999999, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7718), new Guid("11112222-3333-4444-5555-666677778888"), "", 6.7999999999999998, 0, 1.2, 0.14999999999999999, new Guid("b1111111-1111-1111-1111-111111111111"), 0.02, 0.0, 1500.0, 20.0, 7.2000000000000002, 0.10000000000000001, 2.1000000000000001, 0.0, new Guid("11112222-3333-4444-5555-666677778888"), "First Water def", 500.0, 0 },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), 8.0, 0.14999999999999999, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(253), new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "", 4.0, 2, 2.5, 0.5, new Guid("b2222222-2222-2222-2222-222222222222"), 0.10000000000000001, 0.0, 4000.0, 50.0, 6.5, 0.80000000000000004, 5.5, 0.0, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "Second Water def", 800.0, 0 },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), 2.0, 0.01, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(262), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 7.5, 1, 0.90000000000000002, 0.050000000000000003, new Guid("b3333333-3333-3333-3333-333333333333"), 0.0050000000000000001, 0.0, 800.0, 10.0, 8.0, 0.050000000000000003, 1.0, 0.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Water def", 350.0, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

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
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "NatLocations",
                keyColumn: "Id",
                keyValue: new Guid("b6666666-6666-6666-6666-666666666666"));

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
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "OrgLocations",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
