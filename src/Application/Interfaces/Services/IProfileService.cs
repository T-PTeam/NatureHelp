using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IProfileService
{
    Task<bool> TryAwardDailyVisitAsync(Guid userId, CancellationToken ct = default);
    Task<bool> TryAwardPhotoAddedAsync(Guid userId, Guid attachmentId, CancellationToken ct = default);
    Task<bool> TryAwardDeficiencyEditedAsync(Guid userId, Guid changedModelLogId, CancellationToken ct = default);
    Task<DeficiencyConfirmResult> ConfirmDeficiencyAsync(Guid userId, Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default);
    Task<(int Level, int XpIntoLevel, int XpToNext)> GetLevelProgressAsync(int totalXp, CancellationToken ct = default);
    Task<int> RecalculateLevelAsync(int totalXp, CancellationToken ct = default);
}

public sealed record DeficiencyConfirmResult(bool Success, bool AlreadyConfirmed, bool CreatorRewarded, string? ErrorMessage);
