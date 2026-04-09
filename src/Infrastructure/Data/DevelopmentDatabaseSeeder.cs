using Domain.Enums;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DevelopmentDatabaseSeeder : IDevelopmentDatabaseSeeder
{
    private static readonly Guid SeedActorId = Guid.Parse("11112222-3333-4444-5555-666677778888");
    private static readonly Guid OrgGlobalResearch = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid OrgTechHub = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    private static readonly Guid LabBiomedical = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
    private static readonly Guid LabAi = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    private static readonly Guid LabGenetics = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
    private static readonly Guid UserValentynId = Guid.Parse("11112222-3333-4444-5555-666677778888");
    private static readonly Guid UserIgorZId = Guid.Parse("99990000-aaaa-bbbb-cccc-ddddeeeeffff");
    private static readonly Guid UserKaterynaId = Guid.Parse("11223344-5566-7788-99aa-bbccddeeff00");

    private const string HashValentyn =
        "AQAAAAIAAYagAAAAECguO79y3aAyVPpzpWncaB4IYu9PYjpnVFccaS8craV/lS2/wsFIdGgP3zt57jcgng==";
    private const string HashIgorZayets =
        "AQAAAAIAAYagAAAAEAvDOvE1RJIgnTiRC1b1t8ovIg71oxhDmkd+tdUk85PBDMsoLY1lk5hiNFi2OI54yw==";
    private const string HashIgorExample =
        "AQAAAAIAAYagAAAAEKxFyghqrxHSumgKLFEzw7dG6LzDHXmxeuQErcXaVxRD8l7pFWl/gJI94vUXdtBUHw==";

    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public DevelopmentDatabaseSeeder(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        if (await context.Organizations.AnyAsync(o => o.Id == OrgGlobalResearch, cancellationToken))
        {
            return;
        }

        var t0 = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8020);
        var t1 = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8414);

        var orgGlobal = new Organization
        {
            Id = OrgGlobalResearch,
            Title = "Global Research Institute",
            CreatedBy = SeedActorId,
            CreatedOn = t0,
            AllowedMembersCount = 11,
        };
        var orgTech = new Organization
        {
            Id = OrgTechHub,
            Title = "International Tech Hub",
            CreatedBy = SeedActorId,
            CreatedOn = t1,
            AllowedMembersCount = 11,
        };
        context.Organizations.AddRange(orgGlobal, orgTech);

        var labBio = new Laboratory
        {
            Id = LabBiomedical,
            Title = "Biomedical Research Lab",
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(8668),
            Latitude = 50.4501,
            Longitude = 30.5234,
            Address = "Kyiv, Ukraine",
            IsPublic = true,
        };
        var labAi = new Laboratory
        {
            Id = LabAi,
            Title = "AI and Machine Learning Lab",
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9007),
            Latitude = 52.52,
            Longitude = 13.405,
            Address = "Berlin, Germany",
            IsPublic = true,
        };
        var labGen = new Laboratory
        {
            Id = LabGenetics,
            Title = "Genetics and Biotechnology Lab",
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9011),
            Latitude = 40.7128,
            Longitude = -74.006,
            Address = "New York, USA",
            IsPublic = true,
        };
        context.Laboratories.AddRange(labBio, labAi, labGen);

        var uValentyn = new User
        {
            Id = UserValentynId,
            FirstName = "Valentyn",
            LastName = "Riabinchak",
            DateOfBirth = new DateTime(1985, 5, 19, 21, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+380501234567",
            Email = "valentyn@example.com",
            PasswordHash = HashValentyn,
            LaboratoryId = LabBiomedical,
            OrganizationId = OrgGlobalResearch,
            CreatedBy = UserValentynId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc).AddTicks(9649),
            IsEmailConfirmed = true,
        };
        uValentyn.AssignRole(ERole.Supervisor);

        var uIgorZ = new User
        {
            Id = UserIgorZId,
            FirstName = "Valentyn",
            LastName = "Riabinchak",
            DateOfBirth = new DateTime(1990, 7, 14, 21, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+380631234567",
            Email = "igorzayets@example.com",
            PasswordHash = HashIgorZayets,
            LaboratoryId = LabAi,
            OrganizationId = OrgTechHub,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 536, DateTimeKind.Utc).AddTicks(4675),
            IsEmailConfirmed = true,
        };
        uIgorZ.AssignRole(ERole.Supervisor);

        var uKateryna = new User
        {
            Id = UserKaterynaId,
            FirstName = "Igor",
            LastName = "Zaitsev",
            DateOfBirth = new DateTime(1980, 3, 9, 22, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "+49 17612345678",
            Email = "igor@example.com",
            PasswordHash = HashIgorExample,
            LaboratoryId = LabGenetics,
            OrganizationId = OrgTechHub,
            CreatedBy = SeedActorId,
            CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 585, DateTimeKind.Utc).AddTicks(5636),
            IsEmailConfirmed = true,
        };
        uKateryna.AssignRole(ERole.Supervisor);

        context.Users.AddRange(uValentyn, uIgorZ, uKateryna);

        context.Reports.AddRange(
            new Report
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Title = "Annual Genetic Study",
                Topic = EReportTopic.Soil,
                Data = "Genetic research data goes here...",
                ReporterId = UserValentynId,
                CreatedBy = SeedActorId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(5064),
            },
            new Report
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                Title = "AI Algorithm Performance",
                Topic = EReportTopic.Soil,
                Data = "Performance analysis data goes here...",
                ReporterId = UserIgorZId,
                CreatedBy = SeedActorId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6146),
            },
            new Report
            {
                Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                Title = "Global Pandemic Analysis",
                Topic = EReportTopic.Water,
                Data = "Pandemic analysis data goes here...",
                ReporterId = UserKaterynaId,
                CreatedBy = SeedActorId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(6152),
            });

        context.SoilDeficiencies.AddRange(
            new SoilDeficiency
            {
                Id = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                Title = "First Soil def",
                Description = "",
                Type = EDeficiencyType.Soil,
                PH = 6.5,
                OrganicMatter = 3.8,
                LeadConcentration = 150.0,
                CadmiumConcentration = 1.2,
                MercuryConcentration = 0.6,
                PesticidesContent = 0.8,
                NitratesConcentration = 45.0,
                HeavyMetalsConcentration = 120.0,
                ElectricalConductivity = 0.7,
                EDangerState = EDangerState.Moderate,
                MicrobialActivity = 3200,
                AnalysisDate = new DateTime(2025, 1, 14, 22, 0, 0, DateTimeKind.Utc),
                ResponsibleUserId = UserKaterynaId,
                CreatedBy = UserIgorZId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(599),
                Latitude = 50.450099999999999,
                Longitude = 30.523399999999999,
                RadiusAffected = 10,
            },
            new SoilDeficiency
            {
                Id = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                Title = "Second Soil def",
                Description = "",
                Type = EDeficiencyType.Soil,
                PH = 5.9,
                OrganicMatter = 2.5,
                LeadConcentration = 250.0,
                CadmiumConcentration = 2.5,
                MercuryConcentration = 1.1,
                PesticidesContent = 1.5,
                NitratesConcentration = 60.0,
                HeavyMetalsConcentration = 200.0,
                ElectricalConductivity = 0.9,
                EDangerState = EDangerState.Critical,
                MicrobialActivity = 1500,
                AnalysisDate = new DateTime(2025, 1, 17, 22, 0, 0, DateTimeKind.Utc),
                CreatedBy = UserIgorZId,
                ResponsibleUserId = UserKaterynaId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2791),
                Latitude = 49.993499999999997,
                Longitude = 36.229199999999999,
                RadiusAffected = 10,
            },
            new SoilDeficiency
            {
                Id = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                Title = "Third Soil def",
                Description = "",
                Type = EDeficiencyType.Soil,
                PH = 7.2,
                OrganicMatter = 4.1,
                LeadConcentration = 80.0,
                CadmiumConcentration = 0.8,
                MercuryConcentration = 0.3,
                PesticidesContent = 0.5,
                NitratesConcentration = 30.0,
                HeavyMetalsConcentration = 50.0,
                ElectricalConductivity = 0.5,
                EDangerState = EDangerState.Dangerous,
                MicrobialActivity = 4000,
                AnalysisDate = new DateTime(2025, 1, 19, 22, 0, 0, DateTimeKind.Utc),
                CreatedBy = UserIgorZId,
                ResponsibleUserId = UserKaterynaId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(2808),
                Latitude = 48.464700000000001,
                Longitude = 35.0456,
                RadiusAffected = 10,
            });

        context.WaterDeficiencies.AddRange(
            new WaterDeficiency
            {
                Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                Title = "First Water def",
                Description = "",
                Type = EDeficiencyType.Water,
                PH = 7.2,
                DissolvedOxygen = 6.8,
                BiologicalOxygenDemand = 4.5,
                NitrateConcentration = 20.0,
                PhosphateConcentration = 2.1,
                LeadConcentration = 0.15,
                MercuryConcentration = 0.02,
                CadmiumConcentration = 0.03,
                PesticidesContent = 0.1,
                TotalDissolvedSolids = 500.0,
                ElectricalConductivity = 1.2,
                EDangerState = EDangerState.Moderate,
                MicrobialLoad = 1500,
                MicrobialActivity = 0,
                RadiationLevel = 0,
                ChemicalOxygenDemand = 0,
                CreatedBy = UserValentynId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 634, DateTimeKind.Utc).AddTicks(7718),
                ResponsibleUserId = UserValentynId,
                Latitude = 50.450099999999999,
                Longitude = 30.523399999999999,
                RadiusAffected = 10,
            },
            new WaterDeficiency
            {
                Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                Title = "Second Water def",
                Description = "",
                Type = EDeficiencyType.Water,
                PH = 6.5,
                DissolvedOxygen = 4.0,
                BiologicalOxygenDemand = 8.0,
                NitrateConcentration = 50.0,
                PhosphateConcentration = 5.5,
                LeadConcentration = 0.5,
                MercuryConcentration = 0.1,
                CadmiumConcentration = 0.15,
                PesticidesContent = 0.8,
                TotalDissolvedSolids = 800.0,
                ElectricalConductivity = 2.5,
                EDangerState = EDangerState.Critical,
                MicrobialLoad = 4000,
                MicrobialActivity = 0,
                RadiationLevel = 0,
                ChemicalOxygenDemand = 0,
                CreatedBy = UserKaterynaId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(253),
                ResponsibleUserId = UserIgorZId,
                Latitude = 49.8429,
                Longitude = 24.031600000000001,
                RadiusAffected = 10,
            },
            new WaterDeficiency
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                Title = "Third Water def",
                Description = "",
                Type = EDeficiencyType.Water,
                PH = 8.0,
                DissolvedOxygen = 7.5,
                BiologicalOxygenDemand = 2.0,
                NitrateConcentration = 10.0,
                PhosphateConcentration = 1.0,
                LeadConcentration = 0.05,
                MercuryConcentration = 0.005,
                CadmiumConcentration = 0.01,
                PesticidesContent = 0.05,
                TotalDissolvedSolids = 350.0,
                ElectricalConductivity = 0.9,
                EDangerState = EDangerState.Dangerous,
                MicrobialLoad = 800,
                MicrobialActivity = 0,
                RadiationLevel = 0,
                ChemicalOxygenDemand = 0,
                CreatedBy = UserIgorZId,
                CreatedOn = new DateTime(2025, 3, 19, 9, 6, 35, 635, DateTimeKind.Utc).AddTicks(262),
                ResponsibleUserId = UserKaterynaId,
                Latitude = 46.482500000000002,
                Longitude = 30.732600000000001,
                RadiusAffected = 10,
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}
