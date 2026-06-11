using Domain.Enums;
using Domain.Models.Profile;
using Domain.Models.Nature;
using Shared.Dtos;

namespace Infrastructure.Interfaces;

public interface IProfileRepository
{
    Task<bool> HasDailyVisitAsync(Guid userId, DateOnly visitDate, CancellationToken ct = default);
    Task AddDailyVisitAsync(UserDailyVisit visit, CancellationToken ct = default);
    Task<bool> HasConfirmationAsync(Guid userId, Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default);
    Task<int> CountConfirmationsAsync(Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default);
    Task AddConfirmationAsync(DeficiencyConfirmation confirmation, CancellationToken ct = default);
    Task<bool> HasLedgerEntryAsync(Guid userId, XpReason reason, string? referenceType, Guid? referenceId, CancellationToken ct = default);
    Task AddLedgerEntryAsync(UserXpLedger entry, CancellationToken ct = default);
    Task<WaterDeficiency?> GetWaterDeficiencyByIdAsync(Guid id, CancellationToken ct = default);
    Task<SoilDeficiency?> GetSoilDeficiencyByIdAsync(Guid id, CancellationToken ct = default);
    Task<int> CountWaterByCreatorAsync(Guid userId, CancellationToken ct = default);
    Task<int> CountSoilByCreatorAsync(Guid userId, CancellationToken ct = default);
    Task<int> CountConfirmationsGivenByUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> CreatorHasDeficiencyWithFiveConfirmationsAsync(Guid creatorId, CancellationToken ct = default);
    Task<List<Achievement>> GetActiveAchievementsAsync(CancellationToken ct = default);
    Task<UserAchievement?> GetUserAchievementAsync(Guid userId, Guid achievementId, CancellationToken ct = default);
    Task UpsertUserAchievementAsync(UserAchievement ua, CancellationToken ct = default);
    Task<List<DeficiencyAttachment>> GetAttachmentsByCreatorAsync(Guid userId, int take, CancellationToken ct = default);
    Task<List<ProfileJournalEntryDto>> GetProfileJournalAsync(Guid userId, int take, CancellationToken ct = default);
    Task<List<ProfileReferralDto>> GetReferralsAsync(Guid userId, CancellationToken ct = default);
    Task<int> CountReferralsAsync(Guid userId, CancellationToken ct = default);
    Task<string> EnsureReferralCodeAsync(Guid userId, CancellationToken ct = default);
}
