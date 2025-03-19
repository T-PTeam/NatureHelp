using Domain.Enums;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Providers;

public static class GenerateTestDataToDB
{
    private static PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public static List<Domain.Models.Organization.Location> Locations = new List<Domain.Models.Organization.Location>
{
    new Domain.Models.Organization.Location
    {
        Id = new Guid("11111111-1111-1111-1111-111111111111"),
        Country = "Ukraine",
        Region = "Kyiv",
        District = "Shevchenkivsky",
        City = "Kyiv"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("22222222-2222-2222-2222-222222222222"),
        Country = "USA",
        Region = "New York",
        District = "Manhattan",
        City = "New York"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("33333333-3333-3333-3333-333333333333"),
        Country = "Germany",
        Region = "Berlin",
        District = "Mitte",
        City = "Berlin"
    },
};

    public static List<Domain.Models.Organization.Location> PersonLocations = new List<Domain.Models.Organization.Location>
{
    new Domain.Models.Organization.Location
    {
        Id = new Guid("44444444-4444-4444-4444-444444444444"),
        Country = "Ukraine",
        Region = "Kyiv Oblast",
        District = "Kyiv City District",
        City = "Kyiv"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("55555555-5555-5555-5555-555555555555"),
        Country = "USA",
        Region = "New York State",
        District = "Manhattan District",
        City = "New York"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("66666666-6666-6666-6666-666666666666"),
        Country = "Germany",
        Region = "Berlin",
        District = "Mitte District",
        City = "Berlin"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("77777777-7777-7777-7777-777777777777"),
        Country = "Brazil",
        Region = "Rio de Janeiro State",
        District = "Central District",
        City = "Rio de Janeiro"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("88888888-8888-8888-8888-888888888888"),
        Country = "France",
        Region = "Île-de-France",
        District = "Paris City District",
        City = "Paris"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("99999999-9999-9999-9999-999999999999"),
        Country = "Japan",
        Region = "Kantō",
        District = "Tokyo Metropolis District",
        City = "Tokyo"
    },
    new Domain.Models.Organization.Location
    {
        Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Country = "Italy",
        Region = "Lazio",
        District = "Rome City District",
        City = "Rome"
    },
};

    public static List<Organization> Organizations = new List<Organization>
{
    new Organization
    {
        Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        Title = "Global Research Institute",
        LocationId = new Guid("11111111-1111-1111-1111-111111111111"),
    },
    new Organization
    {
        Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
        Title = "International Tech Hub",
        LocationId = new Guid("33333333-3333-3333-3333-333333333333"),
    }
};

    public static List<Laboratory> Laboratories = new List<Laboratory>
{
    new Laboratory
    {
        Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
        Title = "Biomedical Research Lab",
        LocationId = new Guid("11111111-1111-1111-1111-111111111111"),
    },
    new Laboratory
    {
        Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
        Title = "AI and Machine Learning Lab",
        LocationId = new Guid("33333333-3333-3333-3333-333333333333"),
    },
    new Laboratory
    {
        Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
        Title = "Genetics and Biotechnology Lab",
        Researchers = null,
        LocationId = new Guid("22222222-2222-2222-2222-222222222222"),
    }
};

    public static List<User> Users = new List<User>
{
    new User
    {
        Id = new Guid("11112222-3333-4444-5555-666677778888"),
        FirstName = "Valentyn",
        LastName = "Riabinchak",
        DateOfBirth = new DateTime(1985, 5, 20).ToUniversalTime(),
        PhoneNumber = "+380501234567",
        AddressId = new Guid("44444444-4444-4444-4444-444444444444"),
        Email = "valentyn@example.com",
        PasswordHash = _passwordHasher.HashPassword(new User
            {
                Id = new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"),
                FirstName = "Valentyn",
                LastName = "Riabinchak",
                DateOfBirth = new DateTime(1990, 7, 15).ToUniversalTime(),
                PhoneNumber = "+380631234567",
                AddressId = new Guid("55555555-5555-5555-5555-555555555555"),
                Email = "valentyn@example.com",
                Password = "12341234",
                LaboratoryId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                OrganizationId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
            }, "12341234"),
        LaboratoryId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
        OrganizationId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
    },
    new User
    {
        Id = new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"),
        FirstName = "Igor",
        LastName = "Zayets",
        DateOfBirth = new DateTime(1990, 7, 15).ToUniversalTime(),
        PhoneNumber = "+380631234567",
        AddressId = new Guid("55555555-5555-5555-5555-555555555555"),
        Email = "igorzayets@example.com",
        PasswordHash = _passwordHasher.HashPassword(new User
            {
                Id = new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"),
                FirstName = "Igor",
                LastName = "Zayets",
                DateOfBirth = new DateTime(1990, 7, 15).ToUniversalTime(),
                PhoneNumber = "+380631234567",
                AddressId = new Guid("55555555-5555-5555-5555-555555555555"),
                Email = "igorzayets@example.com",
                Password = "10101010",
                LaboratoryId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                OrganizationId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
            }, "10101010"),
        LaboratoryId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
        OrganizationId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
    },
    new User
    {
        Id = new Guid("11223344-5566-7788-99aa-bbccddeeff00"),
        FirstName = "Kateryna",
        LastName = "Morozenko",
        DateOfBirth = new DateTime(1980, 3, 10).ToUniversalTime(),
        PhoneNumber = "+49 17612345678",
        AddressId = new Guid("66666666-6666-6666-6666-666666666666"),
        Email = "katerynamoroz@example.com@example.com",
        PasswordHash = _passwordHasher.HashPassword(new User
            {
                Id = new Guid("99990000-aaaa-bbbb-cccc-ddddeeeeffff"),
                FirstName = "Kateryna",
                LastName = "Morozenko",
                DateOfBirth = new DateTime(1990, 7, 15).ToUniversalTime(),
                PhoneNumber = "+380631234567",
                AddressId = new Guid("55555555-5555-5555-5555-555555555555"),
                Email = "katerynamoroz@example.com",
                Password = "12345678",
                LaboratoryId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                OrganizationId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
            }, "12345678"),
        LaboratoryId = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
        OrganizationId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
    }
};


    public static List<Report> Reports = new List<Report>
{
    new Report
    {
        Id = new Guid("a1111111-1111-1111-1111-111111111111"),
        Title = "Annual Genetic Study",
        Topic = EReportTopic.Soil,
        Data = "Genetic research data goes here...",
        ReporterId = Users[0].Id
    },
    new Report
    {
        Id = new Guid("a2222222-2222-2222-2222-222222222222"),
        Title = "AI Algorithm Performance",
        Topic = EReportTopic.Soil,
        Data = "Performance analysis data goes here...",
        ReporterId = Users[1].Id
    },
    new Report
    {
        Id = new Guid("a3333333-3333-3333-3333-333333333333"),
        Title = "Global Pandemic Analysis",
        Topic = EReportTopic.Water,
        Data = "Pandemic analysis data goes here...",
        ReporterId = Users[2].Id
    }
};

    public static List<Domain.Models.Nature.Location> NatureLocations = new List<Domain.Models.Nature.Location>
{

    new Domain.Models.Nature.Location { Id = new Guid("b1111111-1111-1111-1111-111111111111"), Country = "Ethiopia", City = "Addis Ababa", Longitude = 30.5234, Latitude = 50.4501
},
    new Domain.Models.Nature.Location { Id = new Guid("b2222222-2222-2222-2222-222222222222"), Country = "India", City = "Mumbai" , Longitude = 24.0316, Latitude = 49.8429},
    new Domain.Models.Nature.Location { Id = new Guid("b3333333-3333-3333-3333-333333333333"), Country = "USA", City = "Phoenix", Longitude = 30.7326, Latitude = 46.4825 },
    new Domain.Models.Nature.Location { Id = new Guid("b4444444-4444-4444-4444-444444444444"), Country = "Australia", City = "Sydney" , Longitude = 30.5234, Latitude = 50.4501},
    new Domain.Models.Nature.Location { Id = new Guid("b5555555-5555-5555-5555-555555555555"), Country = "China", City = "Beijing", Longitude = 36.2292, Latitude = 49.9935 },
    new Domain.Models.Nature.Location { Id = new Guid("b6666666-6666-6666-6666-666666666666"), Country = "Ukraine", City = "Uzhhorod" , Longitude = 35.0456, Latitude = 48.4647}
};

    public static List<WaterDeficiency> WaterDeficiencies = new List<WaterDeficiency>
{
    new WaterDeficiency
    {
        Id = new Guid("c1111111-1111-1111-1111-111111111111"),
        Title = "First Water def",
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
        CreatorId = Users[0].Id,
        ResponsibleUserId = Users[0].Id,
        LocationId = NatureLocations[0].Id,
    },
    new WaterDeficiency
    {
        Id = new Guid("c2222222-2222-2222-2222-222222222222"),
        Title = "Second Water def",
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
        CreatorId = Users[2].Id,
        ResponsibleUserId = Users[1].Id,
        LocationId = NatureLocations[1].Id,
    },
    new WaterDeficiency
    {
        Id = new Guid("c3333333-3333-3333-3333-333333333333"),
        Title = "Third Water def",
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
        MicrobialLoad = 800,
        CreatorId = Users[1].Id,
        ResponsibleUserId = Users[2].Id,
        LocationId = NatureLocations[2].Id,
        EDangerState = EDangerState.Dangerous
    }
};

    public static List<SoilDeficiency> SoilDeficiencies = new List<SoilDeficiency>
{
    new SoilDeficiency
    {
        Id = new Guid("d1111111-1111-1111-1111-111111111111"),
        Title = "First Soil def",
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
        AnalysisDate = new DateTime(2025, 1, 15).ToUniversalTime(),
        CreatorId = Users[1].Id,
        ResponsibleUserId = Users[2].Id,
        LocationId = NatureLocations[3].Id,
    },
    new SoilDeficiency
    {
        Id = new Guid("d2222222-2222-2222-2222-222222222222"),
        Title = "Second Soil def",
        PH = 5.9,
        OrganicMatter = 2.5,
        LeadConcentration = 250.0,
        CadmiumConcentration = 2.5,
        MercuryConcentration = 1.1,
        PesticidesContent = 1.5,
        NitratesConcentration = 60.0,
        HeavyMetalsConcentration = 200.0,
        ElectricalConductivity = 0.9,
        MicrobialActivity = 1500,
        AnalysisDate = new DateTime(2025, 1, 18).ToUniversalTime(),
        CreatorId = Users[1].Id,
        ResponsibleUserId = Users[2].Id,
        LocationId = NatureLocations[4].Id,
        EDangerState = EDangerState.Critical,
    },
    new SoilDeficiency
    {
        Id = new Guid("d3333333-3333-3333-3333-333333333333"),
        Title = "Third Soil def",
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
        AnalysisDate = new DateTime(2025, 1, 20).ToUniversalTime(),
        CreatorId = Users[1].Id,
        ResponsibleUserId = Users[2].Id,
        LocationId = NatureLocations[5].Id,
    }
};

}
