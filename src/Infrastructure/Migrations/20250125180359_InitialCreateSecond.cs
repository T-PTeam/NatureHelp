using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NatLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    RadiusAffected = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    Country = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Laboratories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laboratories_OrgLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "OrgLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_OrgLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "OrgLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    AccessTokenExpireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
                    LaboratoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Laboratories_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "Laboratories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_OrgLocations_AddressId",
                        column: x => x.AddressId,
                        principalTable: "OrgLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Topic = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Users_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilDeficiencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PH = table.Column<double>(type: "double precision", nullable: false),
                    OrganicMatter = table.Column<double>(type: "double precision", nullable: false),
                    LeadConcentration = table.Column<double>(type: "double precision", nullable: false),
                    CadmiumConcentration = table.Column<double>(type: "double precision", nullable: false),
                    MercuryConcentration = table.Column<double>(type: "double precision", nullable: false),
                    PesticidesContent = table.Column<double>(type: "double precision", nullable: false),
                    NitratesConcentration = table.Column<double>(type: "double precision", nullable: false),
                    HeavyMetalsConcentration = table.Column<double>(type: "double precision", nullable: false),
                    ElectricalConductivity = table.Column<double>(type: "double precision", nullable: false),
                    ToxicityLevel = table.Column<string>(type: "text", nullable: false),
                    MicrobialActivity = table.Column<double>(type: "double precision", nullable: false),
                    AnalysisDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsibleUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EDangerState = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilDeficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoilDeficiencies_NatLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "NatLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoilDeficiencies_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoilDeficiencies_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WaterDeficiencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PH = table.Column<double>(type: "double precision", nullable: false),
                    DissolvedOxygen = table.Column<double>(type: "double precision", nullable: false),
                    LeadConcentration = table.Column<double>(type: "double precision", nullable: false),
                    MercuryConcentration = table.Column<double>(type: "double precision", nullable: false),
                    NitrateConcentration = table.Column<double>(type: "double precision", nullable: false),
                    PesticidesContent = table.Column<double>(type: "double precision", nullable: false),
                    MicrobialActivity = table.Column<double>(type: "double precision", nullable: false),
                    RadiationLevel = table.Column<double>(type: "double precision", nullable: false),
                    ChemicalOxygenDemand = table.Column<double>(type: "double precision", nullable: false),
                    BiologicalOxygenDemand = table.Column<double>(type: "double precision", nullable: false),
                    ToxicityLevel = table.Column<string>(type: "text", nullable: false),
                    PhosphateConcentration = table.Column<double>(type: "double precision", nullable: false),
                    CadmiumConcentration = table.Column<double>(type: "double precision", nullable: false),
                    TotalDissolvedSolids = table.Column<double>(type: "double precision", nullable: false),
                    ElectricalConductivity = table.Column<double>(type: "double precision", nullable: false),
                    MicrobialLoad = table.Column<double>(type: "double precision", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsibleUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EDangerState = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterDeficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterDeficiencies_NatLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "NatLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaterDeficiencies_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaterDeficiencies_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "NatLocations",
                columns: new[] { "Id", "City", "Country", "CreatedBy", "CreatedOn", "RadiusAffected" },
                values: new object[,]
                {
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Addis Ababa", "Ethiopia", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Mumbai", "India", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), "Phoenix", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), "Sydney", "Australia", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), "Beijing", "China", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), "Uzhhorod", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 }
                });

            migrationBuilder.InsertData(
                table: "OrgLocations",
                columns: new[] { "Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Kyiv", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shevchenkivsky", "Kyiv" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "New York", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manhattan", "New York" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Berlin", "Germany", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mitte", "Berlin" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Kyiv", "Ukraine", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kyiv City District", "Kyiv Oblast" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "New York", "USA", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manhattan District", "New York State" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Berlin", "Germany", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mitte District", "Berlin" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Rio de Janeiro", "Brazil", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Central District", "Rio de Janeiro State" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Paris", "France", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paris City District", "Île-de-France" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Tokyo", "Japan", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tokyo Metropolis District", "Kantō" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Rome", "Italy", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rome City District", "Lazio" }
                });

            migrationBuilder.InsertData(
                table: "Laboratories",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LocationId", "Title" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "Biomedical Research Lab" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), "AI and Machine Learning Lab" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("22222222-2222-2222-2222-222222222222"), "Genetics and Biotechnology Lab" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LocationId", "Title" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "Global Research Institute" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), "International Tech Hub" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessToken", "AccessTokenExpireTime", "AddressId", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "LaboratoryId", "LastName", "OrganizationId", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime" },
                values: new object[,]
                {
                    { new Guid("11112222-3333-4444-5555-666677778888"), null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 5, 19, 21, 0, 0, 0, DateTimeKind.Utc), "valentyn@example.com", "Valentyn", new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Riabinchak", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "hashed_password_1", "+380501234567", null, null },
                    { new Guid("11223344-5566-7788-99aa-bbccddeeff00"), null, null, new Guid("66666666-6666-6666-6666-666666666666"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 3, 9, 22, 0, 0, 0, DateTimeKind.Utc), "igor@example.com", "Igor", new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Zaitsev", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "hashed_password_3", "+49 17612345678", null, null },
                    { new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), null, null, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 7, 14, 21, 0, 0, 0, DateTimeKind.Utc), "vasylyna@example.com", "Vasylyna", new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Leheta", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "hashed_password_2", "+380631234567", null, null }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Genetic research data goes here...", new Guid("11112222-3333-4444-5555-666677778888"), "Annual Genetic Study", 1 },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Performance analysis data goes here...", new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "AI Algorithm Performance", 1 },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pandemic analysis data goes here...", new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Global Pandemic Analysis", 0 }
                });

            migrationBuilder.InsertData(
                table: "SoilDeficiencies",
                columns: new[] { "Id", "AnalysisDate", "CadmiumConcentration", "CreatedBy", "CreatedOn", "CreatorId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "ResponsibleUserId", "Title", "ToxicityLevel", "Type" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 14, 22, 0, 0, 0, DateTimeKind.Utc), 1.2, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 0, 0.69999999999999996, 120.0, 150.0, new Guid("b4444444-4444-4444-4444-444444444444"), 0.59999999999999998, 3200.0, 45.0, 3.7999999999999998, 6.5, 0.80000000000000004, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "First Soil def", "Moderate", 0 },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 17, 22, 0, 0, 0, DateTimeKind.Utc), 2.5, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 0, 0.90000000000000002, 200.0, 250.0, new Guid("b5555555-5555-5555-5555-555555555555"), 1.1000000000000001, 1500.0, 60.0, 2.5, 5.9000000000000004, 1.5, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Second Soil def", "High", 0 },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), new DateTime(2025, 1, 19, 22, 0, 0, 0, DateTimeKind.Utc), 0.80000000000000004, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 0, 0.5, 50.0, 80.0, new Guid("b6666666-6666-6666-6666-666666666666"), 0.29999999999999999, 4000.0, 30.0, 4.0999999999999996, 7.2000000000000002, 0.5, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Soil def", "Low", 0 }
                });

            migrationBuilder.InsertData(
                table: "WaterDeficiencies",
                columns: new[] { "Id", "BiologicalOxygenDemand", "CadmiumConcentration", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "CreatorId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "ToxicityLevel", "Type" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), 4.5, 0.029999999999999999, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11112222-3333-4444-5555-666677778888"), "", 6.7999999999999998, 0, 1.2, 0.14999999999999999, new Guid("b1111111-1111-1111-1111-111111111111"), 0.02, 0.0, 1500.0, 20.0, 7.2000000000000002, 0.10000000000000001, 2.1000000000000001, 0.0, new Guid("11112222-3333-4444-5555-666677778888"), "First Water def", 500.0, "Moderate", 0 },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), 8.0, 0.14999999999999999, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "", 4.0, 0, 2.5, 0.5, new Guid("b2222222-2222-2222-2222-222222222222"), 0.10000000000000001, 0.0, 4000.0, 50.0, 6.5, 0.80000000000000004, 5.5, 0.0, new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "Second Water def", 800.0, "High", 0 },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), 2.0, 0.01, 0.0, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"), "", 7.5, 0, 0.90000000000000002, 0.050000000000000003, new Guid("b3333333-3333-3333-3333-333333333333"), 0.0050000000000000001, 0.0, 800.0, 10.0, 8.0, 0.050000000000000003, 1.0, 0.0, new Guid("11223344-5566-7788-99aa-bbccddeeff00"), "Third Water def", 350.0, "Low", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_LocationId",
                table: "Laboratories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LocationId",
                table: "Organizations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_CreatorId",
                table: "SoilDeficiencies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_LocationId",
                table: "SoilDeficiencies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilDeficiencies_ResponsibleUserId",
                table: "SoilDeficiencies",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LaboratoryId",
                table: "Users",
                column: "LaboratoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_CreatorId",
                table: "WaterDeficiencies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_LocationId",
                table: "WaterDeficiencies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterDeficiencies_ResponsibleUserId",
                table: "WaterDeficiencies",
                column: "ResponsibleUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SoilDeficiencies");

            migrationBuilder.DropTable(
                name: "WaterDeficiencies");

            migrationBuilder.DropTable(
                name: "NatLocations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Laboratories");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "OrgLocations");
        }
    }
}
