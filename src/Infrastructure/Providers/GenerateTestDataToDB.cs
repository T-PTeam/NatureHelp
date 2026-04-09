using Domain.Enums;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;

namespace Infrastructure.Providers;

public static class GenerateTestDataToDB
{
    public static readonly Guid SeedActorId = new("11112222-3333-4444-5555-666677778888");

    private static readonly Guid OrgGlobalId = new("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid OrgTechHubId = new("cccccccc-cccc-cccc-cccc-cccccccccccc");
    private static readonly Guid LabBioId = new("dddddddd-dddd-dddd-dddd-dddddddddddd");
    private static readonly Guid LabAiId = new("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    private static readonly Guid LabGeneticsId = new("ffffffff-ffff-ffff-ffff-ffffffffffff");
    private static readonly Guid UserValentynId = new("11112222-3333-4444-5555-666677778888");
    private static readonly Guid UserIgorZId = new("99990000-aaaa-bbbb-cccc-ddddeeeeffff");
    private static readonly Guid UserIgorExampleId = new("11223344-5566-7788-99aa-bbccddeeff00");

    private static readonly DateTime OrgGlobalCreatedOn =
        new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8020);
    private static readonly DateTime OrgTechCreatedOn =
        new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8414);
    private static readonly DateTime LabBioCreatedOn =
        new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8668);
    private static readonly DateTime LabAiCreatedOn =
        new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9007);
    private static readonly DateTime LabGeneticsCreatedOn =
        new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9011);

    public static readonly IReadOnlyList<Organization> Organizations =
    [
        new Organization
        {
            Id = OrgGlobalId,
            Title = "Global Research Institute",
            CreatedBy = SeedActorId,
            CreatedOn = OrgGlobalCreatedOn,
            AllowedMembersCount = 11,
        },
        new Organization
        {
            Id = OrgTechHubId,
            Title = "International Tech Hub",
            CreatedBy = SeedActorId,
            CreatedOn = OrgTechCreatedOn,
            AllowedMembersCount = 11,
        },
    ];

    public static readonly IReadOnlyList<Laboratory> Laboratories =
    [
        new Laboratory
        {
            Id = LabBioId,
            Title = "Biomedical Research Lab",
            CreatedBy = SeedActorId,
            CreatedOn = LabBioCreatedOn,
        },
        new Laboratory
        {
            Id = LabAiId,
            Title = "AI and Machine Learning Lab",
            CreatedBy = SeedActorId,
            CreatedOn = LabAiCreatedOn,
        },
        new Laboratory
        {
            Id = LabGeneticsId,
            Title = "Genetics and Biotechnology Lab",
            CreatedBy = SeedActorId,
            CreatedOn = LabGeneticsCreatedOn,
            Researchers = null,
        },
    ];

    public static readonly IReadOnlyList<User> Users =
    [
        new User
        {
            Id = UserValentynId,
            FirstName = "Valentyn",
            LastName = "Riabinchak",
            DateOfBirth = new DateTime(1985, 5, 19, 21, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+380501234567",
            Email = "valentyn@example.com",
            PasswordHash =
                "AQAAAAIAAYagAAAAECguO79y3aAyVPpzpWncaB4IYu9PYjpnVFccaS8craV/lS2/wsFIdGgP3zt57jcgng==",
            LaboratoryId = LabBioId,
            OrganizationId = OrgGlobalId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9649),
            IsEmailConfirmed = true,
        },
        new User
        {
            Id = UserIgorExampleId,
            FirstName = "Igor",
            LastName = "Zaitsev",
            DateOfBirth = new DateTime(1980, 3, 9, 22, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+49 17612345678",
            Email = "igor@example.com",
            PasswordHash =
                "AQAAAAIAAYagAAAAEKxFyghqrxHSumgKLFEzw7dG6LzDHXmxeuQErcXaVxRD8l7pFWl/gJI94vUXdtBUHw==",
            LaboratoryId = LabGeneticsId,
            OrganizationId = OrgTechHubId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 585, DateTimeKind.Utc).AddTicks(5636),
            IsEmailConfirmed = true,
        },
        new User
        {
            Id = UserIgorZId,
            FirstName = "Valentyn",
            LastName = "Riabinchak",
            DateOfBirth = new DateTime(1990, 7, 14, 21, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+380631234567",
            Email = "igorzayets@example.com",
            PasswordHash =
                "AQAAAAIAAYagAAAAEAvDOvE1RJIgnTiRC1b1t8ovIg71oxhDmkd+tdUk85PBDMsoLY1lk5hiNFi2OI54yw==",
            LaboratoryId = LabAiId,
            OrganizationId = OrgTechHubId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 536, DateTimeKind.Utc).AddTicks(4675),
            IsEmailConfirmed = true,
        },
    ];

    public static readonly IReadOnlyList<Report> Reports =
    [
        new Report
        {
            Id = new Guid("a1111111-1111-1111-1111-111111111111"),
            Title = "Annual Genetic Study",
            Topic = EReportTopic.Soil,
            Data = "Genetic research data goes here...",
            ReporterId = UserValentynId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(5064),
        },
        new Report
        {
            Id = new Guid("a2222222-2222-2222-2222-222222222222"),
            Title = "AI Algorithm Performance",
            Topic = EReportTopic.Soil,
            Data = "Performance analysis data goes here...",
            ReporterId = UserIgorZId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6146),
        },
        new Report
        {
            Id = new Guid("a3333333-3333-3333-3333-333333333333"),
            Title = "Global Pandemic Analysis",
            Topic = EReportTopic.Water,
            Data = "Pandemic analysis data goes here...",
            ReporterId = UserIgorExampleId,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6152),
        },
    ];

    public static readonly IReadOnlyList<SoilDeficiency> SoilDeficiencies =
    [
        new SoilDeficiency
        {
            Id = new Guid("d1111111-1111-1111-1111-111111111111"),
            Title = "First Soil def",
            Description = "",
            Type = EDeficiencyType.Soil,
            PH = 6.5,
            OrganicMatter = 3.7999999999999998,
            LeadConcentration = 150.0,
            CadmiumConcentration = 1.2,
            MercuryConcentration = 0.59999999999999998,
            PesticidesContent = 0.80000000000000004,
            NitratesConcentration = 45.0,
            HeavyMetalsConcentration = 120.0,
            ElectricalConductivity = 0.69999999999999996,
            EDangerState = EDangerState.Moderate,
            MicrobialActivity = 3200.0,
            AnalysisDate = new DateTime(2025, 1, 14, 22, 0, 0, DateTimeKind.Utc),
            CreatedBy = UserIgorZId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(599),
            ResponsibleUserId = UserIgorExampleId,
            Latitude = 50.450099999999999,
            Longitude = 30.523399999999999,
            RadiusAffected = 10.0,
        },
        new SoilDeficiency
        {
            Id = new Guid("d2222222-2222-2222-2222-222222222222"),
            Title = "Second Soil def",
            Description = "",
            Type = EDeficiencyType.Soil,
            PH = 5.9000000000000004,
            OrganicMatter = 2.5,
            LeadConcentration = 250.0,
            CadmiumConcentration = 2.5,
            MercuryConcentration = 1.1000000000000001,
            PesticidesContent = 1.5,
            NitratesConcentration = 60.0,
            HeavyMetalsConcentration = 200.0,
            ElectricalConductivity = 0.90000000000000002,
            EDangerState = EDangerState.Critical,
            MicrobialActivity = 1500.0,
            AnalysisDate = new DateTime(2025, 1, 17, 22, 0, 0, DateTimeKind.Utc),
            CreatedBy = UserIgorZId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2791),
            ResponsibleUserId = UserIgorExampleId,
            Latitude = 49.993499999999997,
            Longitude = 36.229199999999999,
            RadiusAffected = 10.0,
        },
        new SoilDeficiency
        {
            Id = new Guid("d3333333-3333-3333-3333-333333333333"),
            Title = "Third Soil def",
            Description = "",
            Type = EDeficiencyType.Soil,
            PH = 7.2000000000000002,
            OrganicMatter = 4.0999999999999996,
            LeadConcentration = 80.0,
            CadmiumConcentration = 0.80000000000000004,
            MercuryConcentration = 0.29999999999999999,
            PesticidesContent = 0.5,
            NitratesConcentration = 30.0,
            HeavyMetalsConcentration = 50.0,
            ElectricalConductivity = 0.5,
            EDangerState = EDangerState.Dangerous,
            MicrobialActivity = 4000.0,
            AnalysisDate = new DateTime(2025, 1, 19, 22, 0, 0, DateTimeKind.Utc),
            CreatedBy = UserIgorZId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2808),
            ResponsibleUserId = UserIgorExampleId,
            Latitude = 48.464700000000001,
            Longitude = 35.0456,
            RadiusAffected = 10.0,
        },
    ];

    public static readonly IReadOnlyList<WaterDeficiency> WaterDeficiencies =
    [
        new WaterDeficiency
        {
            Id = new Guid("c1111111-1111-1111-1111-111111111111"),
            Title = "First Water def",
            Description = "",
            Type = EDeficiencyType.Water,
            PH = 7.2000000000000002,
            DissolvedOxygen = 6.7999999999999998,
            LeadConcentration = 0.14999999999999999,
            MercuryConcentration = 0.02,
            NitrateConcentration = 20.0,
            PesticidesContent = 0.10000000000000001,
            MicrobialActivity = 0.0,
            RadiationLevel = 0.0,
            ChemicalOxygenDemand = 0.0,
            BiologicalOxygenDemand = 4.5,
            PhosphateConcentration = 2.1000000000000001,
            CadmiumConcentration = 0.029999999999999999,
            TotalDissolvedSolids = 500.0,
            ElectricalConductivity = 1.2,
            MicrobialLoad = 1500.0,
            EDangerState = EDangerState.Moderate,
            CreatedBy = UserValentynId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7718),
            ResponsibleUserId = UserValentynId,
            Latitude = 50.450099999999999,
            Longitude = 30.523399999999999,
            RadiusAffected = 10.0,
        },
        new WaterDeficiency
        {
            Id = new Guid("c2222222-2222-2222-2222-222222222222"),
            Title = "Second Water def",
            Description = "",
            Type = EDeficiencyType.Water,
            PH = 6.5,
            DissolvedOxygen = 4.0,
            LeadConcentration = 0.5,
            MercuryConcentration = 0.10000000000000001,
            NitrateConcentration = 50.0,
            PesticidesContent = 0.80000000000000004,
            MicrobialActivity = 0.0,
            RadiationLevel = 0.0,
            ChemicalOxygenDemand = 0.0,
            BiologicalOxygenDemand = 8.0,
            PhosphateConcentration = 5.5,
            CadmiumConcentration = 0.14999999999999999,
            TotalDissolvedSolids = 800.0,
            ElectricalConductivity = 2.5,
            MicrobialLoad = 4000.0,
            EDangerState = EDangerState.Critical,
            CreatedBy = UserIgorExampleId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(253),
            ResponsibleUserId = UserIgorZId,
            Latitude = 49.8429,
            Longitude = 24.031600000000001,
            RadiusAffected = 10.0,
        },
        new WaterDeficiency
        {
            Id = new Guid("c3333333-3333-3333-3333-333333333333"),
            Title = "Third Water def",
            Description = "",
            Type = EDeficiencyType.Water,
            PH = 8.0,
            DissolvedOxygen = 7.5,
            LeadConcentration = 0.050000000000000003,
            MercuryConcentration = 0.0050000000000000001,
            NitrateConcentration = 10.0,
            PesticidesContent = 0.050000000000000003,
            MicrobialActivity = 0.0,
            RadiationLevel = 0.0,
            ChemicalOxygenDemand = 0.0,
            BiologicalOxygenDemand = 2.0,
            PhosphateConcentration = 1.0,
            CadmiumConcentration = 0.01,
            TotalDissolvedSolids = 350.0,
            ElectricalConductivity = 0.90000000000000002,
            MicrobialLoad = 800.0,
            EDangerState = EDangerState.Dangerous,
            CreatedBy = UserIgorZId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(262),
            ResponsibleUserId = UserIgorExampleId,
            Latitude = 46.482500000000002,
            Longitude = 30.732600000000001,
            RadiusAffected = 10.0,
        },
    ];
}
