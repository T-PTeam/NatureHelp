using Domain.Enums;

namespace Infrastructure.Data;

internal static class DevelopmentSeedData
{
    public const string OwnerValentynPassword = "12341234";
    public const string DefaultDemoPassword = "DemoPass1!";

    public static readonly Guid SeedActorId = Guid.Parse("11112222-3333-4444-5555-666677778888");

    public static readonly Guid OrgUkrainianInstitute = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid OrgRiverBasinAlliance = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    public static readonly Guid OrgCarpathianCooperative = Guid.Parse("dd111111-dddd-dddd-dddd-dddddddddd11");

    public static readonly Guid LabKyivSoil = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
    public static readonly Guid LabDniproWater = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    public static readonly Guid LabOdesaCoastal = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
    public static readonly Guid LabKharkivIndustrial = Guid.Parse("aa111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid LabLvivFreshwater = Guid.Parse("bb222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid LabVinnytsiaAgro = Guid.Parse("cc333333-cccc-cccc-cccc-cccccccccccc");

    public static readonly Guid UserValentynId = Guid.Parse("11112222-3333-4444-5555-666677778888");
    public static readonly Guid UserIgorZId = Guid.Parse("99990000-aaaa-bbbb-cccc-ddddeeeeffff");
    public static readonly Guid UserKaterynaId = Guid.Parse("11223344-5566-7788-99aa-bbccddeeff00");
    public static readonly Guid UserAndriiId = Guid.Parse("22334455-6677-8899-aabb-ccddeeff0011");
    public static readonly Guid UserOlenaId = Guid.Parse("33445566-7788-99aa-bbcc-ddeeff001122");
    public static readonly Guid UserMykolaId = Guid.Parse("44556677-8899-aabb-ccdd-eeff00112233");
    public static readonly Guid UserNataliaId = Guid.Parse("55667788-99aa-bbcc-cdd0-0eff11223344");
    public static readonly Guid UserDmytroId = Guid.Parse("66778899-aabb-ccdd-ee00-ff1122334455");

    public static readonly string[] SeedOwnerEmails =
    [
        "valentyn@example.com",
        "igorzayets@example.com",
    ];

    public static readonly SeedOrganization[] Organizations =
    [
        new(
            OrgUkrainianInstitute,
            "Ukrainian Environmental Research Institute",
            "National research body coordinating soil, water, and biodiversity monitoring across central and eastern Ukraine.",
            15),
        new(
            OrgRiverBasinAlliance,
            "Black Sea & River Basin Monitoring Alliance",
            "Cross-regional coalition focused on Dnipro, Southern Bug, and Black Sea coastal water quality.",
            12),
        new(
            OrgCarpathianCooperative,
            "Carpathian Nature Protection Cooperative",
            "Community-led network of field labs covering western Ukrainian rivers, forests, and mountain catchments.",
            10),
    ];

    public static readonly SeedLaboratory[] Laboratories =
    [
        new(LabKyivSoil, "Kyiv Soil & Agrochemistry Laboratory", 50.4547, 30.5238, "14 Hlybochytska St, Kyiv, Ukraine"),
        new(LabDniproWater, "Dnipro River Hydrochemistry Laboratory", 48.4640, 35.0465, "27 Naberezhna Peremohy, Dnipro, Ukraine"),
        new(LabOdesaCoastal, "Odesa Coastal & Marine Laboratory", 46.4775, 30.7326, "4 Lanzheron Beach Rd, Odesa, Ukraine"),
        new(LabKharkivIndustrial, "Kharkiv Industrial Ecology Laboratory", 49.9800, 36.2527, "8 Nauky Ave, Kharkiv, Ukraine"),
        new(LabLvivFreshwater, "Lviv Freshwater Monitoring Laboratory", 49.8397, 24.0297, "11 Kopernyka St, Lviv, Ukraine"),
        new(LabVinnytsiaAgro, "Vinnytsia Agroecology Field Laboratory", 49.2331, 28.4682, "45 Soborna St, Vinnytsia, Ukraine"),
    ];

    public static readonly SeedUser[] CoreUsers =
    [
        new(UserValentynId, "Valentyn", "Riabinchak", "valentyn@example.com", OwnerValentynPassword, ERole.Owner,
            OrgUkrainianInstitute, LabKyivSoil, new DateTime(1985, 5, 19, 0, 0, 0, DateTimeKind.Utc), "+380501234567"),
        new(UserIgorZId, "Ihor", "Zaiets", "igorzayets@example.com", DefaultDemoPassword, ERole.Owner,
            OrgRiverBasinAlliance, LabOdesaCoastal, new DateTime(1990, 7, 14, 0, 0, 0, DateTimeKind.Utc), "+380631234567"),
        new(UserKaterynaId, "Kateryna", "Melnyk", "kateryna.melnyk@example.com", DefaultDemoPassword, ERole.Supervisor,
            OrgUkrainianInstitute, LabDniproWater, new DateTime(1988, 3, 9, 0, 0, 0, DateTimeKind.Utc), "+380671112233"),
        new(UserAndriiId, "Andrii", "Bondarenko", "andrii.bondarenko@example.com", DefaultDemoPassword, ERole.Researcher,
            OrgUkrainianInstitute, LabKharkivIndustrial, new DateTime(1992, 11, 2, 0, 0, 0, DateTimeKind.Utc), "+380931234501"),
        new(UserOlenaId, "Olena", "Shevchenko", "olena.shevchenko@example.com", DefaultDemoPassword, ERole.Researcher,
            OrgRiverBasinAlliance, LabDniproWater, new DateTime(1994, 6, 18, 0, 0, 0, DateTimeKind.Utc), "+380661234502"),
        new(UserMykolaId, "Mykola", "Koval", "mykola.koval@example.com", DefaultDemoPassword, ERole.Supervisor,
            OrgCarpathianCooperative, LabLvivFreshwater, new DateTime(1983, 9, 25, 0, 0, 0, DateTimeKind.Utc), "+380501234503"),
        new(UserNataliaId, "Natalia", "Petrenko", "natalia.petrenko@example.com", DefaultDemoPassword, ERole.Researcher,
            OrgUkrainianInstitute, LabVinnytsiaAgro, new DateTime(1996, 1, 30, 0, 0, 0, DateTimeKind.Utc), "+380971234504"),
        new(UserDmytroId, "Dmytro", "Ivanov", "dmytro.ivanov@example.com", DefaultDemoPassword, ERole.Manager,
            OrgRiverBasinAlliance, LabOdesaCoastal, new DateTime(1987, 8, 7, 0, 0, 0, DateTimeKind.Utc), "+380681234505"),
    ];

    public static readonly SeedBulkUserProfile[] BulkUserProfiles =
    [
        new("Iryna", "Lysenko", "iryna.lysenko@example.com"),
        new("Vasyl", "Tkachenko", "vasyl.tkachenko@example.com"),
        new("Hanna", "Moroz", "hanna.moroz@example.com"),
        new("Taras", "Savchuk", "taras.savchuk@example.com"),
        new("Yuliia", "Kravets", "yuliia.kravets@example.com"),
        new("Petro", "Hryhoriev", "petro.hryhoriev@example.com"),
        new("Sofiia", "Romaniuk", "sofiia.romaniuk@example.com"),
        new("Roman", "Danylenko", "roman.danylenko@example.com"),
        new("Viktoriia", "Ponomarenko", "viktoriia.ponomarenko@example.com"),
        new("Bohdan", "Kuzmenko", "bohdan.kuzmenko@example.com"),
        new("Lesia", "Holub", "lesia.holub@example.com"),
        new("Oleksandr", "Marchenko", "oleksandr.marchenko@example.com"),
    ];

    public static readonly string[] BulkLaboratoryTitles =
    [
        "Poltava Steppe Ecology Station",
        "Chernihiv Floodplain Research Unit",
        "Zaporizhzhia Industrial Impact Lab",
        "Mykolaiv Estuary Monitoring Post",
        "Uzhhorod Mountain Stream Lab",
        "Chernivtsi Prut River Field Station",
        "Sumy Agricultural Runoff Lab",
        "Zhytomyr Forest Soil Observatory",
        "Rivne Wetland Assessment Unit",
        "Kropyvnytskyi Irrigation Canal Lab",
        "Khmelnytskyi Reservoir Watchpoint",
        "Ternopil Pond Ecosystem Station",
    ];

    public static readonly SeedLocation[] UkrainianLocations =
    [
        new("Kyiv, Hydropark embankment", 50.4432, 30.5711, "Kyiv"),
        new("Kyiv, Troieshchyna industrial zone", 50.5183, 30.6024, "Kyiv"),
        new("Kharkiv, Lisovy Park buffer zone", 50.0254, 36.2281, "Kharkiv"),
        new("Kharkiv, Nemyshlianskyi pond", 49.9587, 36.3124, "Kharkiv"),
        new("Dnipro, Monastyrskyi Island shore", 48.4551, 35.0702, "Dnipro"),
        new("Dnipro, Prydniprovskyi Chemical Plant area", 48.5128, 35.0816, "Dnipro"),
        new("Odesa, Lanzheron beach", 46.4775, 30.7326, "Odesa"),
        new("Odesa, Khadzhibey estuary", 46.4182, 30.6541, "Odesa"),
        new("Lviv, Poltva river section", 49.8356, 24.0243, "Lviv"),
        new("Lviv, Stryiskyi Park meadow", 49.8191, 24.0388, "Lviv"),
        new("Vinnytsia, Southern Bug tributary", 49.2214, 28.4921, "Vinnytsia"),
        new("Poltava, Vorskla river floodplain", 49.5712, 34.5128, "Poltava"),
        new("Chernihiv, Desna riverbank", 51.4918, 31.2894, "Chernihiv"),
        new("Zaporizhzhia, Dnipro rapids near Khortytsia", 47.8445, 35.0952, "Zaporizhzhia"),
        new("Mykolaiv, Buh estuary", 46.9581, 31.9946, "Mykolaiv"),
        new("Uzhhorod, Uzh river urban section", 48.6208, 22.2879, "Uzhhorod"),
        new("Chernivtsi, Prut riverside park", 48.2917, 25.9352, "Chernivtsi"),
        new("Bila Tserkva, Ros river meadow", 49.8012, 30.1124, "Kyiv Oblast"),
        new("Konotop, railway depot vicinity", 51.2403, 33.2028, "Sumy Oblast"),
        new("Sloviansk, Siverskyi Donets crossing", 48.8571, 37.6083, "Donetsk Oblast"),
        new("Voznesensk, Southern Bug gorge", 47.5612, 31.3348, "Mykolaiv Oblast"),
        new("Izmail, Danube delta boundary", 45.3371, 28.8124, "Odesa Oblast"),
        new("Kamianets-Podilskyi, Smotrych canyon", 48.6765, 26.5851, "Khmelnytskyi Oblast"),
        new("Kovel, Turia river wetland", 51.2154, 24.7082, "Volyn Oblast"),
        new("Kremenchuk, Dnipro reservoir shore", 49.0654, 33.4201, "Poltava Oblast"),
        new("Berlin, Spree river park", 52.5200, 13.4050, "Berlin"),
        new("Warsaw, Vistula boulevard", 52.2297, 21.0122, "Warsaw"),
        new("Prague, Vltava riverside", 50.0755, 14.4378, "Prague"),
    ];

    public static readonly SeedSoilDeficiencyTemplate[] CoreSoilDeficiencies =
    [
        new(
            Guid.Parse("d1111111-1111-1111-1111-111111111111"),
            "Heavy metals near Troieshchyna industrial zone",
            "Composite soil samples from the former machine-building yards show elevated lead and cadmium. Residents reported dust during dry winds; remediation is recommended before any residential expansion.",
            EDangerState.Moderate,
            50.5183, 30.6024, "Troieshchyna industrial zone, Kyiv, Ukraine", 3.5,
            6.2, 2.8, 185.0, 1.4, 0.7, 1.1, 52.0, 145.0, 0.82, 2800,
            UserKaterynaId, UserIgorZId),
        new(
            Guid.Parse("d2222222-2222-2222-2222-222222222222"),
            "Nitrate buildup in Kharkiv Lisovy Park buffer soils",
            "Repeated fertilizer drift from nearby greenhouse complexes increased nitrate levels in topsoil. Tree root zones may be stressed; monitoring advised each spring.",
            EDangerState.Critical,
            50.0254, 36.2281, "Lisovy Park buffer zone, Kharkiv, Ukraine", 2.8,
            5.9, 2.1, 95.0, 0.9, 0.3, 0.6, 78.0, 68.0, 0.55, 4100,
            UserAndriiId, UserKaterynaId),
        new(
            Guid.Parse("d3333333-3333-3333-3333-333333333333"),
            "Legacy contamination at Prydniprovskyi Chemical Plant",
            "Soil cores collected 800 m from the plant fence line contain high concentrations of mercury and mixed heavy metals linked to historical spills. Access paths should remain restricted.",
            EDangerState.Critical,
            48.5128, 35.0816, "Prydniprovskyi Chemical Plant area, Dnipro, Ukraine", 4.0,
            6.8, 1.9, 320.0, 3.1, 1.8, 2.2, 41.0, 265.0, 1.15, 1200,
            UserOlenaId, UserIgorZId),
        new(
            Guid.Parse("d4444444-4444-4444-4444-444444444444"),
            "Pesticide residues in Bila Tserkva cropland",
            "Spring wheat fields show persistent organophosphate traces after three seasons of intensive spraying. Organic matter is still high, but microbial activity declined compared with reference plots.",
            EDangerState.Dangerous,
            49.8012, 30.1124, "Ros river meadows, Bila Tserkva, Ukraine", 3.0,
            7.0, 4.2, 72.0, 0.7, 0.2, 1.8, 34.0, 55.0, 0.48, 3600,
            UserNataliaId, UserValentynId),
        new(
            Guid.Parse("d5555555-5555-5555-5555-555555555555"),
            "Oil hydrocarbons near Konotop railway depot",
            "Surface soil along the loading bay contains diesel-range hydrocarbons and reduced microbial activity. A localized excavation and bioremediation pilot is under discussion with municipal services.",
            EDangerState.Moderate,
            51.2403, 33.2028, "Konotop railway depot vicinity, Sumy Oblast, Ukraine", 2.5,
            6.5, 3.1, 110.0, 1.0, 0.4, 0.9, 28.0, 98.0, 0.63, 2200,
            UserMykolaId, UserAndriiId),
        new(
            Guid.Parse("d6666666-6666-6666-6666-666666666666"),
            "Copper accumulation on Uzhhorod vineyard terraces",
            "Long-term fungicide use left moderate copper and zinc levels on hillside terraces. Grapevines still appear healthy, but runoff during heavy rain may affect the Uzh river section below.",
            EDangerState.Moderate,
            48.6124, 22.3012, "Vineyard terraces above Uzh river, Uzhhorod, Ukraine", 2.2,
            6.9, 3.6, 48.0, 0.5, 0.1, 0.4, 18.0, 72.0, 0.41, 3900,
            UserMykolaId, UserNataliaId),
    ];

    public static readonly SeedWaterDeficiencyTemplate[] CoreWaterDeficiencies =
    [
        new(
            Guid.Parse("c1111111-1111-1111-1111-111111111111"),
            "Urban runoff at Kyiv Hydropark embankment",
            "After spring storms, phosphate and nitrate spikes appear near storm drains entering the Dnipro. Dissolved oxygen recovers within 48 hours but weekend bathing advisories are occasionally issued.",
            EDangerState.Moderate,
            50.4432, 30.5711, "Hydropark embankment, Kyiv, Ukraine", 2.5,
            7.1, 6.4, 4.8, 18.0, 1.6, 0.008, 0.004, 0.002, 0.04, 420.0, 0.95, 1200, 180, 12.0,
            UserValentynId, UserValentynId),
        new(
            Guid.Parse("c2222222-2222-2222-2222-222222222222"),
            "Agricultural nitrate pulse on Southern Bug near Voznesensk",
            "Melting snow and early irrigation produced a nitrate pulse exceeding local guidelines. Downstream fishers reported temporary algae films; follow-up sampling scheduled after dam release.",
            EDangerState.Dangerous,
            47.5612, 31.3348, "Southern Bug gorge, Voznesensk, Ukraine", 3.2,
            7.4, 5.8, 6.2, 42.0, 2.4, 0.006, 0.003, 0.004, 0.02, 510.0, 1.35, 2100, 240, 18.0,
            UserOlenaId, UserKaterynaId),
        new(
            Guid.Parse("c3333333-3333-3333-3333-333333333333"),
            "Post-storm bacterial load at Lanzheron beach",
            "Combined sewer overflow after heavy rain raised microbial counts along the public beach. City services posted temporary swimming restrictions until two consecutive clean samples are recorded.",
            EDangerState.Critical,
            46.4775, 30.7326, "Lanzheron beach, Odesa, Ukraine", 2.0,
            7.8, 4.2, 9.5, 12.0, 3.8, 0.004, 0.002, 0.003, 0.06, 680.0, 2.1, 4200, 320, 28.0,
            UserIgorZId, UserDmytroId),
        new(
            Guid.Parse("c4444444-4444-4444-4444-444444444444"),
            "Industrial runoff in Siverskyi Donets near Sloviansk",
            "Elevated BOD and iron hydroxide staining were observed downstream of a disused processing site. Local water utility intakes upstream remain within safe limits.",
            EDangerState.Dangerous,
            48.8571, 37.6083, "Siverskyi Donets crossing, Sloviansk, Ukraine", 3.8,
            6.9, 3.6, 11.0, 28.0, 1.9, 0.009, 0.005, 0.004, 0.08, 740.0, 1.85, 3600, 410, 32.0,
            UserAndriiId, UserIgorZId),
        new(
            Guid.Parse("c5555555-5555-5555-5555-555555555555"),
            "Phosphate enrichment at Danube delta boundary",
            "Upstream agricultural drainage increased phosphate levels at the biosphere reserve boundary. Wetland managers requested coordinated sampling with Romanian partners.",
            EDangerState.Critical,
            45.3371, 28.8124, "Danube delta boundary, Izmail, Ukraine", 4.5,
            8.1, 5.1, 7.8, 8.0, 4.6, 0.003, 0.001, 0.002, 0.01, 390.0, 0.88, 1800, 150, 22.0,
            UserDmytroId, UserOlenaId),
        new(
            Guid.Parse("c6666666-6666-6666-6666-666666666666"),
            "Municipal discharge impact on Ros river near Bila Tserkva",
            "Intermittent ammonia odors and elevated COD were reported near a stormwater outfall. The municipality started separator maintenance; oxygen levels recover within two kilometers downstream.",
            EDangerState.Moderate,
            49.8012, 30.1058, "Ros river near Bila Tserkva, Ukraine", 2.8,
            7.3, 6.9, 3.4, 22.0, 1.2, 0.005, 0.002, 0.002, 0.02, 360.0, 0.72, 900, 120, 14.0,
            UserKaterynaId, UserNataliaId),
    ];

    public static readonly string[] BulkSoilTitles =
    [
        "Compaction and salinity on irrigated plots",
        "Trace metals near abandoned quarry",
        "Acidification in pine forest litter",
        "Erosion gully topsoil loss",
        "Herbicide drift in riverside meadow",
        "Slag heap leachate footprint",
        "Arsenic hotspot near old smelter",
        "Organic matter decline on overgrazed pasture",
    ];

    public static readonly string[] BulkSoilDescriptions =
    [
        "Irrigation without drainage raised electrical conductivity; crop yields already show stress on the lower terrace.",
        "Wind-blown dust from the quarry perimeter enriched surface soils with zinc and manganese.",
        "Needle litter pH dropped below optimal range for understory plants after a dry summer.",
        "A new gully exposed subsoil with low organic matter; sediment may reach the nearest stream during rain.",
        "Selective herbicide applied on adjacent fields left measurable residues in protected meadow soil.",
        "Seasonal leachate from legacy slag spreads a narrow plume downslope toward farmland.",
        "Historical smelting left localized arsenic and lead; fencing was installed pending remediation design.",
        "Continuous grazing reduced topsoil structure; recovery will be tracked against a fenced reference plot.",
    ];

    public static readonly string[] BulkWaterTitles =
    [
        "Algae bloom risk in sheltered bay",
        "Low dissolved oxygen below weir",
        "Sediment plume after bank restoration",
        "Elevated conductivity near marina",
        "Microplastic survey hotspot",
        "Thermal discharge mixing zone",
        "Fuel odor reported near boat ramp",
        "Spring flood nutrient surge",
    ];

    public static readonly string[] BulkWaterDescriptions =
    [
        "Warm, slow-moving water and high phosphates created ideal conditions for cyanobacteria; signage posted for dog owners.",
        "A low-head weir traps organic matter; night-time oxygen sags below the threshold for sensitive fish species.",
        "Recent bank works increased turbidity; benthic invertebrate counts will be repeated after sediment settles.",
        "Marina antifouling maintenance coincided with a conductivity spike; source tracing is in progress.",
        "Surface trawls found elevated microplastic fibers downstream of a textile district outfall.",
        "Cooling water from a small industrial plant creates a visible mixing plume; temperature logged hourly.",
        "A thin petroleum sheen appeared after a weekend boating event; absorbent booms were deployed temporarily.",
        "Snowmelt carried nitrates from upstream fields; utilities adjusted treatment chemical dosing as precaution.",
    ];

    internal sealed record SeedOrganization(Guid Id, string Title, string Description, int AllowedMembersCount);

    internal sealed record SeedLaboratory(Guid Id, string Title, double Latitude, double Longitude, string Address);

    internal sealed record SeedUser(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        ERole Role,
        Guid OrganizationId,
        Guid LaboratoryId,
        DateTime DateOfBirth,
        string PhoneNumber);

    internal sealed record SeedBulkUserProfile(string FirstName, string LastName, string Email);

    internal sealed record SeedLocation(string Address, double Latitude, double Longitude, string Region);

    internal sealed record SeedSoilDeficiencyTemplate(
        Guid Id,
        string Title,
        string Description,
        EDangerState DangerState,
        double Latitude,
        double Longitude,
        string Address,
        double RadiusAffected,
        double Ph,
        double OrganicMatter,
        double LeadConcentration,
        double CadmiumConcentration,
        double MercuryConcentration,
        double PesticidesContent,
        double NitratesConcentration,
        double HeavyMetalsConcentration,
        double ElectricalConductivity,
        double MicrobialActivity,
        Guid ResponsibleUserId,
        Guid CreatedByUserId);

    internal sealed record SeedWaterDeficiencyTemplate(
        Guid Id,
        string Title,
        string Description,
        EDangerState DangerState,
        double Latitude,
        double Longitude,
        string Address,
        double RadiusAffected,
        double Ph,
        double DissolvedOxygen,
        double BiologicalOxygenDemand,
        double NitrateConcentration,
        double PhosphateConcentration,
        double LeadConcentration,
        double MercuryConcentration,
        double CadmiumConcentration,
        double PesticidesContent,
        double TotalDissolvedSolids,
        double ElectricalConductivity,
        double MicrobialLoad,
        double MicrobialActivity,
        double ChemicalOxygenDemand,
        Guid ResponsibleUserId,
        Guid CreatedByUserId);
}
