using Domain.Enums;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Domain.Models.Profile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Infrastructure.Data.DevelopmentSeedData;

namespace Infrastructure.Data;

public class DevelopmentDatabaseSeeder : IDevelopmentDatabaseSeeder
{
    private static readonly PasswordHasher<User> PasswordHasher = new();

    private readonly IDbContextFactory<ApplicationContext> _contextFactory;
    private readonly IConfiguration _configuration;

    public DevelopmentDatabaseSeeder(
        IDbContextFactory<ApplicationContext> contextFactory,
        IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _configuration = configuration;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await EnsureLevelThresholdsDictAsync(context, cancellationToken);

        if (!await context.Organizations.AnyAsync(o => o.Id == OrgUkrainianInstitute, cancellationToken))
        {
            await SeedBaseDataAsync(context, cancellationToken);
        }

        await EnsureBulkDataAsync(context, cancellationToken);
        await EnsureSeedOwnerRolesAsync(context, cancellationToken);
    }

    private static async Task SeedBaseDataAsync(ApplicationContext context, CancellationToken cancellationToken)
    {
        var seedTime = new DateTime(2025, 3, 19, 9, 6, 35, 480, DateTimeKind.Utc);

        foreach (var organization in Organizations)
        {
            context.Organizations.Add(new Organization
            {
                Id = organization.Id,
                Title = organization.Title,
                CreatedBy = SeedActorId,
                CreatedOn = seedTime,
                AllowedMembersCount = organization.AllowedMembersCount,
            });
        }

        foreach (var laboratory in Laboratories)
        {
            context.Laboratories.Add(new Laboratory
            {
                Id = laboratory.Id,
                Title = laboratory.Title,
                CreatedBy = SeedActorId,
                CreatedOn = seedTime,
                Latitude = laboratory.Latitude,
                Longitude = laboratory.Longitude,
                Address = laboratory.Address,
                IsPublic = true,
            });
        }

        foreach (var coreUser in CoreUsers)
        {
            var user = new User
            {
                Id = coreUser.Id,
                FirstName = coreUser.FirstName,
                LastName = coreUser.LastName,
                DateOfBirth = coreUser.DateOfBirth,
                PhoneNumber = coreUser.PhoneNumber,
                Email = coreUser.Email,
                PasswordHash = HashPassword(coreUser.Password),
                LaboratoryId = coreUser.LaboratoryId,
                OrganizationId = coreUser.OrganizationId,
                CreatedBy = coreUser.Id == UserValentynId ? UserValentynId : SeedActorId,
                CreatedOn = seedTime,
                IsEmailConfirmed = true,
            };
            user.AssignRole(coreUser.Role);
            context.Users.Add(user);
        }

        context.Reports.AddRange(
            new Report
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Title = "Chernozem health review for central Ukraine",
                Topic = EReportTopic.Soil,
                Data = "Quarterly comparison of organic matter and nitrate trends across Kyiv, Vinnytsia, and Poltava monitoring plots.",
                ReporterId = UserValentynId,
                CreatedBy = SeedActorId,
                CreatedOn = seedTime,
            },
            new Report
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                Title = "Black Sea bathing water quality summary",
                Topic = EReportTopic.Water,
                Data = "Summer microbial load and dissolved oxygen patterns for Odesa and Mykolaiv coastal stations.",
                ReporterId = UserIgorZId,
                CreatedBy = SeedActorId,
                CreatedOn = seedTime,
            },
            new Report
            {
                Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                Title = "Dnipro basin nutrient transport model",
                Topic = EReportTopic.Water,
                Data = "Estimated phosphate and nitrate inputs from agricultural and municipal sources between Kyiv and Zaporizhzhia.",
                ReporterId = UserKaterynaId,
                CreatedBy = SeedActorId,
                CreatedOn = seedTime,
            });

        foreach (var template in CoreSoilDeficiencies)
        {
            context.SoilDeficiencies.Add(CreateSoilDeficiency(template, seedTime));
        }

        foreach (var template in CoreWaterDeficiencies)
        {
            context.WaterDeficiencies.Add(CreateWaterDeficiency(template, seedTime));
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureBulkDataAsync(ApplicationContext context, CancellationToken cancellationToken)
    {
        var targets = GetSeedTargets();
        await EnsureBulkLaboratoriesAsync(context, targets.Laboratories, cancellationToken);
        await EnsureBulkUsersAsync(context, targets.Users, cancellationToken);
        await EnsureBulkSoilDeficienciesAsync(context, targets.SoilDeficiencies, cancellationToken);
        await EnsureBulkWaterDeficienciesAsync(context, targets.WaterDeficiencies, cancellationToken);
    }

    private SeedTargets GetSeedTargets()
    {
        static int ReadCount(IConfiguration configuration, string key, int defaultValue) =>
            int.TryParse(configuration[key], out var count) && count >= 0 ? count : defaultValue;

        return new SeedTargets
        {
            Users = ReadCount(_configuration, "Seed:UsersCount", 15),
            Laboratories = ReadCount(_configuration, "Seed:LaboratoriesCount", 30),
            SoilDeficiencies = ReadCount(_configuration, "Seed:SoilDeficienciesCount", 50),
            WaterDeficiencies = ReadCount(_configuration, "Seed:WaterDeficienciesCount", 50),
        };
    }

    private static async Task EnsureBulkLaboratoriesAsync(
        ApplicationContext context,
        int targetCount,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.Laboratories.CountAsync(cancellationToken);
        if (existingCount >= targetCount)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var ukraineLocations = UkrainianLocations.Take(UkrainianLocations.Length - 3).ToArray();
        var internationalLocations = UkrainianLocations.TakeLast(3).ToArray();

        for (var index = existingCount + 1; index <= targetCount; index++)
        {
            var id = BulkId(0x20, index);
            if (await context.Laboratories.AnyAsync(l => l.Id == id, cancellationToken))
            {
                continue;
            }

            var useInternational = index > targetCount - 3;
            var location = useInternational
                ? internationalLocations[(index - 1) % internationalLocations.Length]
                : ukraineLocations[(index - 1) % ukraineLocations.Length];
            var title = BulkLaboratoryTitles[(index - 1) % BulkLaboratoryTitles.Length];
            var offset = ((index % 7) - 3) * 0.009;

            context.Laboratories.Add(new Laboratory
            {
                Id = id,
                Title = $"{title} #{index}",
                CreatedBy = SeedActorId,
                CreatedOn = now,
                Latitude = location.Latitude + offset,
                Longitude = location.Longitude + offset,
                Address = location.Address,
                IsPublic = true,
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureBulkUsersAsync(
        ApplicationContext context,
        int targetCount,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.Users.CountAsync(cancellationToken);
        if (existingCount >= targetCount)
        {
            return;
        }

        var laboratoryIds = await context.Laboratories
            .OrderBy(l => l.CreatedOn)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        if (laboratoryIds.Count == 0)
        {
            return;
        }

        var organizationIds = Organizations.Select(o => o.Id).ToArray();
        var now = DateTime.UtcNow;

        for (var index = existingCount + 1; index <= targetCount; index++)
        {
            var id = BulkId(0x10, index);
            if (await context.Users.AnyAsync(u => u.Id == id, cancellationToken))
            {
                continue;
            }

            var profile = BulkUserProfiles[(index - 1) % BulkUserProfiles.Length];
            var user = new User
            {
                Id = id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DateOfBirth = new DateTime(1985 + (index % 15), (index % 12) + 1, (index % 27) + 1, 0, 0, 0, DateTimeKind.Utc),
                PhoneNumber = $"+38050{index:D7}",
                Email = profile.Email,
                PasswordHash = HashPassword(DefaultDemoPassword),
                LaboratoryId = laboratoryIds[(index - 1) % laboratoryIds.Count],
                OrganizationId = organizationIds[(index - 1) % organizationIds.Length],
                CreatedBy = SeedActorId,
                CreatedOn = now,
                IsEmailConfirmed = true,
            };
            user.AssignRole(index % 4 == 0 ? ERole.Supervisor : ERole.Researcher);
            context.Users.Add(user);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureBulkSoilDeficienciesAsync(
        ApplicationContext context,
        int targetCount,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.SoilDeficiencies.CountAsync(cancellationToken);
        if (existingCount >= targetCount)
        {
            return;
        }

        var userIds = await context.Users.Select(u => u.Id).ToListAsync(cancellationToken);
        if (userIds.Count == 0)
        {
            return;
        }

        var dangerStates = new[] { EDangerState.Moderate, EDangerState.Dangerous, EDangerState.Critical };
        var now = DateTime.UtcNow;

        for (var index = existingCount + 1; index <= targetCount; index++)
        {
            var id = BulkId(0x30, index);
            if (await context.SoilDeficiencies.AnyAsync(d => d.Id == id, cancellationToken))
            {
                continue;
            }

            var location = UkrainianLocations[(index - 1) % UkrainianLocations.Length];
            var title = BulkSoilTitles[(index - 1) % BulkSoilTitles.Length];
            var description = BulkSoilDescriptions[(index - 1) % BulkSoilDescriptions.Length];
            var offset = ((index % 7) - 3) * 0.012;
            var responsibleUserId = userIds[(index - 1) % userIds.Count];
            var creatorId = userIds[index % userIds.Count];

            context.SoilDeficiencies.Add(new SoilDeficiency
            {
                Id = id,
                Title = $"{title} — {location.Region}",
                Description = $"{description} Location reference: {location.Address}.",
                Type = EDeficiencyType.Soil,
                PH = 5.5 + (index % 20) * 0.1,
                OrganicMatter = 2.0 + (index % 25) * 0.1,
                LeadConcentration = 50 + index * 3,
                CadmiumConcentration = 0.5 + (index % 10) * 0.1,
                MercuryConcentration = 0.2 + (index % 8) * 0.05,
                PesticidesContent = 0.3 + (index % 6) * 0.1,
                NitratesConcentration = 20 + index,
                HeavyMetalsConcentration = 40 + index * 2,
                ElectricalConductivity = 0.4 + (index % 10) * 0.05,
                MicrobialActivity = 1000 + index * 50,
                AnalysisDate = now.AddDays(-index),
                EDangerState = dangerStates[(index - 1) % dangerStates.Length],
                ResponsibleUserId = responsibleUserId,
                CreatedBy = creatorId,
                CreatedOn = now,
                Latitude = location.Latitude + offset,
                Longitude = location.Longitude + offset,
                Address = location.Address,
                RadiusAffected = 2 + (index % 4),
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureBulkWaterDeficienciesAsync(
        ApplicationContext context,
        int targetCount,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.WaterDeficiencies.CountAsync(cancellationToken);
        if (existingCount >= targetCount)
        {
            return;
        }

        var userIds = await context.Users.Select(u => u.Id).ToListAsync(cancellationToken);
        if (userIds.Count == 0)
        {
            return;
        }

        var dangerStates = new[] { EDangerState.Moderate, EDangerState.Dangerous, EDangerState.Critical };
        var now = DateTime.UtcNow;

        for (var index = existingCount + 1; index <= targetCount; index++)
        {
            var id = BulkId(0x40, index);
            if (await context.WaterDeficiencies.AnyAsync(d => d.Id == id, cancellationToken))
            {
                continue;
            }

            var location = UkrainianLocations[(index + 5) % UkrainianLocations.Length];
            var title = BulkWaterTitles[(index - 1) % BulkWaterTitles.Length];
            var description = BulkWaterDescriptions[(index - 1) % BulkWaterDescriptions.Length];
            var offset = ((index % 5) - 2) * 0.015;
            var responsibleUserId = userIds[(index - 1) % userIds.Count];
            var creatorId = userIds[index % userIds.Count];

            context.WaterDeficiencies.Add(new WaterDeficiency
            {
                Id = id,
                Title = $"{title} — {location.Region}",
                Description = $"{description} Sampling site: {location.Address}.",
                Type = EDeficiencyType.Water,
                PH = 6.0 + (index % 20) * 0.1,
                DissolvedOxygen = 4.0 + (index % 12) * 0.3,
                BiologicalOxygenDemand = 2.0 + (index % 15) * 0.4,
                NitrateConcentration = 5 + (index % 40),
                PhosphateConcentration = 0.2 + (index % 15) * 0.1,
                LeadConcentration = 0.001 + (index % 8) * 0.001,
                MercuryConcentration = 0.0001 + (index % 8) * 0.0001,
                CadmiumConcentration = 0.001 + (index % 4) * 0.001,
                PesticidesContent = 0.001 + (index % 4) * 0.001,
                TotalDissolvedSolids = 200 + index * 8,
                ElectricalConductivity = 0.5 + (index % 20) * 0.1,
                MicrobialLoad = 300 + index * 25,
                MicrobialActivity = 100 + (index % 80) * 10,
                RadiationLevel = index % 8,
                ChemicalOxygenDemand = 5 + (index % 30),
                EDangerState = dangerStates[(index - 1) % dangerStates.Length],
                ResponsibleUserId = responsibleUserId,
                CreatedBy = creatorId,
                CreatedOn = now,
                Latitude = location.Latitude + offset,
                Longitude = location.Longitude + offset,
                Address = location.Address,
                RadiusAffected = 2 + (index % 5),
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureSeedOwnerRolesAsync(
        ApplicationContext context,
        CancellationToken cancellationToken)
    {
        await context.Users
            .Where(u => SeedOwnerEmails.Contains(u.Email) && u.Role != ERole.Owner)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(u => u.Role, ERole.Owner),
                cancellationToken);
    }

    private static SoilDeficiency CreateSoilDeficiency(SeedSoilDeficiencyTemplate template, DateTime createdOn) =>
        new()
        {
            Id = template.Id,
            Title = template.Title,
            Description = template.Description,
            Type = EDeficiencyType.Soil,
            PH = template.Ph,
            OrganicMatter = template.OrganicMatter,
            LeadConcentration = template.LeadConcentration,
            CadmiumConcentration = template.CadmiumConcentration,
            MercuryConcentration = template.MercuryConcentration,
            PesticidesContent = template.PesticidesContent,
            NitratesConcentration = template.NitratesConcentration,
            HeavyMetalsConcentration = template.HeavyMetalsConcentration,
            ElectricalConductivity = template.ElectricalConductivity,
            MicrobialActivity = template.MicrobialActivity,
            AnalysisDate = createdOn.AddDays(-30),
            EDangerState = template.DangerState,
            ResponsibleUserId = template.ResponsibleUserId,
            CreatedBy = template.CreatedByUserId,
            CreatedOn = createdOn,
            Latitude = template.Latitude,
            Longitude = template.Longitude,
            Address = template.Address,
            RadiusAffected = template.RadiusAffected,
        };

    private static WaterDeficiency CreateWaterDeficiency(SeedWaterDeficiencyTemplate template, DateTime createdOn) =>
        new()
        {
            Id = template.Id,
            Title = template.Title,
            Description = template.Description,
            Type = EDeficiencyType.Water,
            PH = template.Ph,
            DissolvedOxygen = template.DissolvedOxygen,
            BiologicalOxygenDemand = template.BiologicalOxygenDemand,
            NitrateConcentration = template.NitrateConcentration,
            PhosphateConcentration = template.PhosphateConcentration,
            LeadConcentration = template.LeadConcentration,
            MercuryConcentration = template.MercuryConcentration,
            CadmiumConcentration = template.CadmiumConcentration,
            PesticidesContent = template.PesticidesContent,
            TotalDissolvedSolids = template.TotalDissolvedSolids,
            ElectricalConductivity = template.ElectricalConductivity,
            MicrobialLoad = template.MicrobialLoad,
            MicrobialActivity = template.MicrobialActivity,
            RadiationLevel = 0,
            ChemicalOxygenDemand = template.ChemicalOxygenDemand,
            EDangerState = template.DangerState,
            ResponsibleUserId = template.ResponsibleUserId,
            CreatedBy = template.CreatedByUserId,
            CreatedOn = createdOn,
            Latitude = template.Latitude,
            Longitude = template.Longitude,
            Address = template.Address,
            RadiusAffected = template.RadiusAffected,
        };

    private static string HashPassword(string password) =>
        PasswordHasher.HashPassword(new User(), password);

    private static Guid BulkId(int prefixByte, int index) =>
        Guid.Parse($"{prefixByte:x2}000000-0000-4000-8000-{index:x12}");

    private sealed class SeedTargets
    {
        public int Users { get; init; }
        public int Laboratories { get; init; }
        public int SoilDeficiencies { get; init; }
        public int WaterDeficiencies { get; init; }
    }

    private static async Task EnsureLevelThresholdsDictAsync(ApplicationContext context, CancellationToken cancellationToken)
    {
        const string key = "level_thresholds";
        if (await context.DictEntries.AnyAsync(e => e.EntryKey == key, cancellationToken))
        {
            return;
        }

        context.DictEntries.Add(new AppDictEntry
        {
            Id = Guid.NewGuid(),
            EntryKey = key,
            ValueJson = "[0,100,250,500,800,1200]",
        });
        await context.SaveChangesAsync(cancellationToken);
    }
}
