using Domain.Enums;
using Domain.Models.Profile;
using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Domain.Models.Organization;

namespace Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public ProfileRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> HasDailyVisitAsync(Guid userId, DateOnly visitDate, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.UserDailyVisits.AsNoTracking()
            .AnyAsync(v => v.UserId == userId && v.VisitDate == visitDate, ct);
    }

    public async Task AddDailyVisitAsync(UserDailyVisit visit, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        ctx.UserDailyVisits.Add(visit);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> HasConfirmationAsync(Guid userId, Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.DeficiencyConfirmations.AsNoTracking()
            .AnyAsync(c => c.UserId == userId && c.DeficiencyId == deficiencyId && c.DeficiencyType == type, ct);
    }

    public async Task<int> CountConfirmationsAsync(Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.DeficiencyConfirmations.AsNoTracking()
            .CountAsync(c => c.DeficiencyId == deficiencyId && c.DeficiencyType == type, ct);
    }

    public async Task AddConfirmationAsync(DeficiencyConfirmation confirmation, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        ctx.DeficiencyConfirmations.Add(confirmation);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> HasLedgerEntryAsync(Guid userId, XpReason reason, string? referenceType, Guid? referenceId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        var q = ctx.UserXpLedgers.AsNoTracking().Where(l => l.UserId == userId && l.Reason == reason);
        if (referenceType != null)
            q = q.Where(l => l.ReferenceType == referenceType);
        else
            q = q.Where(l => l.ReferenceType == null);
        if (referenceId.HasValue)
            q = q.Where(l => l.ReferenceId == referenceId);
        else
            q = q.Where(l => l.ReferenceId == null);
        return await q.AnyAsync(ct);
    }

    public async Task AddLedgerEntryAsync(UserXpLedger entry, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        ctx.UserXpLedgers.Add(entry);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<WaterDeficiency?> GetWaterDeficiencyByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.WaterDeficiencies.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<SoilDeficiency?> GetSoilDeficiencyByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.SoilDeficiencies.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<int> CountWaterByCreatorAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.WaterDeficiencies.AsNoTracking().CountAsync(d => d.CreatedBy == userId, ct);
    }

    public async Task<int> CountSoilByCreatorAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.SoilDeficiencies.AsNoTracking().CountAsync(d => d.CreatedBy == userId, ct);
    }

    public async Task<int> CountConfirmationsGivenByUserAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.DeficiencyConfirmations.AsNoTracking().CountAsync(c => c.UserId == userId, ct);
    }

    public async Task<bool> CreatorHasDeficiencyWithFiveConfirmationsAsync(Guid creatorId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        var waterIds = await ctx.WaterDeficiencies.AsNoTracking()
            .Where(d => d.CreatedBy == creatorId)
            .Select(d => d.Id)
            .ToListAsync(ct);
        foreach (var id in waterIds)
        {
            var n = await ctx.DeficiencyConfirmations.AsNoTracking()
                .CountAsync(c => c.DeficiencyId == id && c.DeficiencyType == EDeficiencyType.Water, ct);
            if (n >= 5) return true;
        }

        var soilIds = await ctx.SoilDeficiencies.AsNoTracking()
            .Where(d => d.CreatedBy == creatorId)
            .Select(d => d.Id)
            .ToListAsync(ct);
        foreach (var id in soilIds)
        {
            var n = await ctx.DeficiencyConfirmations.AsNoTracking()
                .CountAsync(c => c.DeficiencyId == id && c.DeficiencyType == EDeficiencyType.Soil, ct);
            if (n >= 5) return true;
        }

        return false;
    }

    public async Task<List<Achievement>> GetActiveAchievementsAsync(CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.Achievements.AsNoTracking()
            .Where(a => a.IsActive)
            .OrderBy(a => a.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<UserAchievement?> GetUserAchievementAsync(Guid userId, Guid achievementId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.UserAchievements.AsNoTracking()
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId, ct);
    }

    public async Task UpsertUserAchievementAsync(UserAchievement ua, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        var existing = await ctx.UserAchievements.FirstOrDefaultAsync(
            x => x.UserId == ua.UserId && x.AchievementId == ua.AchievementId, ct);
        if (existing == null)
            ctx.UserAchievements.Add(ua);
        else
        {
            existing.Progress = ua.Progress;
            existing.IsCompleted = ua.IsCompleted;
            existing.UnlockedAt = ua.UnlockedAt;
        }

        await ctx.SaveChangesAsync(ct);
    }

    public async Task<List<DeficiencyAttachment>> GetAttachmentsByCreatorAsync(Guid userId, int take, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.Attachments.AsNoTracking()
            .OfType<DeficiencyAttachment>()
            .Where(a => a.CreatedBy == userId)
            .OrderByDescending(a => a.CreatedOn)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task<List<ProfileJournalEntryDto>> GetProfileJournalAsync(Guid userId, int take, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        var water = await ctx.WaterDeficiencies.AsNoTracking()
            .Where(d => d.CreatedBy == userId)
            .Select(d => new ProfileJournalEntryDto
            {
                Id = d.Id,
                DeficiencyType = (int)EDeficiencyType.Water,
                Title = d.Title,
                CreatedOn = d.CreatedOn,
                Address = d.Address,
            })
            .ToListAsync(ct);
        var soil = await ctx.SoilDeficiencies.AsNoTracking()
            .Where(d => d.CreatedBy == userId)
            .Select(d => new ProfileJournalEntryDto
            {
                Id = d.Id,
                DeficiencyType = (int)EDeficiencyType.Soil,
                Title = d.Title,
                CreatedOn = d.CreatedOn,
                Address = d.Address,
            })
            .ToListAsync(ct);
        return water.Concat(soil).OrderByDescending(x => x.CreatedOn).Take(take).ToList();
    }

    public async Task<List<ProfileReferralDto>> GetReferralsAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.Users.AsNoTracking()
            .Where(u => u.ReferredByUserId == userId)
            .Select(u => new ProfileReferralDto
            {
                Id = u.Id,
                Email = u.Email,
                DisplayName = u.FirstName + " " + u.LastName,
                JoinedAt = u.CreatedOn,
                AvatarStage = u.CurrentLevel <= 2 ? "seed"
                    : u.CurrentLevel <= 4 ? "sprout"
                    : u.CurrentLevel <= 6 ? "sapling"
                    : "tree",
            })
            .OrderByDescending(r => r.JoinedAt)
            .ToListAsync(ct);
    }

    public async Task<int> CountReferralsAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.Users.AsNoTracking().CountAsync(u => u.ReferredByUserId == userId, ct);
    }

    public async Task<string> EnsureReferralCodeAsync(Guid userId, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user == null) throw new InvalidOperationException("User not found.");
        if (!string.IsNullOrEmpty(user.ReferralCode)) return user.ReferralCode;
        user.ReferralCode = Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
        await ctx.SaveChangesAsync(ct);
        return user.ReferralCode;
    }
}
