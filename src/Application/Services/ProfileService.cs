using Application.Interfaces.Services;
using Application.Options;
using Domain.Enums;
using Domain.Models.Profile;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ProfileService : IProfileService
{
    public const string RefTypeDeficiency = "Deficiency";
    public const string RefTypeAttachment = "DeficiencyAttachment";
    public const string RefTypeChangeLog = "ChangedModelLog";

    private readonly IProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAchievementEvaluationService _achievementEvaluation;
    private readonly ILevelThresholdsProvider _levelThresholdsProvider;
    private readonly ProfileOptions _options;

    public ProfileService(
        IProfileRepository profileRepository,
        IUserRepository userRepository,
        IAchievementEvaluationService achievementEvaluation,
        ILevelThresholdsProvider levelThresholdsProvider,
        IOptions<ProfileOptions> options)
    {
        _profileRepository = profileRepository;
        _userRepository = userRepository;
        _achievementEvaluation = achievementEvaluation;
        _levelThresholdsProvider = levelThresholdsProvider;
        _options = options.Value;
    }

    public async Task<bool> TryAwardDailyVisitAsync(Guid userId, CancellationToken ct = default)
    {
        var visitDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (await _profileRepository.HasDailyVisitAsync(userId, visitDate, ct))
            return false;

        await _profileRepository.AddDailyVisitAsync(new UserDailyVisit
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VisitDate = visitDate,
        }, ct);

        return await TryAwardXpAsync(userId, _options.DailyVisitXp, XpReason.DailyVisit, null, null, ct);
    }

    public Task<bool> TryAwardPhotoAddedAsync(Guid userId, Guid attachmentId, CancellationToken ct = default) =>
        TryAwardXpAsync(userId, _options.PhotoAddedXp, XpReason.PhotoAddedToDeficiency, RefTypeAttachment, attachmentId, ct);

    public Task<bool> TryAwardDeficiencyEditedAsync(Guid userId, Guid changedModelLogId, CancellationToken ct = default) =>
        TryAwardXpAsync(userId, _options.DeficiencyEditedXp, XpReason.DeficiencyEdited, RefTypeChangeLog, changedModelLogId, ct);

    public async Task<DeficiencyConfirmResult> ConfirmDeficiencyAsync(Guid userId, Guid deficiencyId, EDeficiencyType type, CancellationToken ct = default)
    {
        Guid creatorId;
        if (type == EDeficiencyType.Water)
        {
            var d = await _profileRepository.GetWaterDeficiencyByIdAsync(deficiencyId, ct);
            if (d == null) return new DeficiencyConfirmResult(false, false, false, "Deficiency not found");
            creatorId = d.CreatedBy;
        }
        else
        {
            var d = await _profileRepository.GetSoilDeficiencyByIdAsync(deficiencyId, ct);
            if (d == null) return new DeficiencyConfirmResult(false, false, false, "Deficiency not found");
            creatorId = d.CreatedBy;
        }

        if (creatorId == userId)
            return new DeficiencyConfirmResult(false, false, false, "Cannot confirm your own deficiency");

        if (await _profileRepository.HasConfirmationAsync(userId, deficiencyId, type, ct))
            return new DeficiencyConfirmResult(true, true, false, null);

        await _profileRepository.AddConfirmationAsync(new DeficiencyConfirmation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DeficiencyId = deficiencyId,
            DeficiencyType = type,
            CreatedOn = DateTime.UtcNow,
        }, ct);

        await _achievementEvaluation.EvaluateForUserAsync(userId, ct);

        var count = await _profileRepository.CountConfirmationsAsync(deficiencyId, type, ct);
        var creatorRewarded = false;
        if (count >= 5)
        {
            creatorRewarded = await TryAwardXpAsync(
                creatorId,
                _options.DeficiencyApproved,
                XpReason.DeficiencyApproved,
                RefTypeDeficiency,
                deficiencyId,
                ct);
        }

        return new DeficiencyConfirmResult(true, false, creatorRewarded, null);
    }

    public async Task<(int Level, int XpIntoLevel, int XpToNext)> GetLevelProgressAsync(int totalXp, CancellationToken ct = default)
    {
        var thresholds = await _levelThresholdsProvider.GetCumulativeThresholdsAsync(ct);
        return LevelProgressCalculator.ComputeProgress(totalXp, thresholds);
    }

    public async Task<int> RecalculateLevelAsync(int totalXp, CancellationToken ct = default)
    {
        var thresholds = await _levelThresholdsProvider.GetCumulativeThresholdsAsync(ct);
        return LevelProgressCalculator.ComputeLevel(totalXp, thresholds);
    }

    private async Task<bool> TryAwardXpAsync(
        Guid userId,
        int amount,
        XpReason reason,
        string? referenceType,
        Guid? referenceId,
        CancellationToken ct)
    {
        if (amount == 0) return false;

        if (await _profileRepository.HasLedgerEntryAsync(userId, reason, referenceType, referenceId, ct))
            return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        await _profileRepository.AddLedgerEntryAsync(new UserXpLedger
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = amount,
            Reason = reason,
            ReferenceType = referenceType,
            ReferenceId = referenceId,
            CreatedOn = DateTime.UtcNow,
        }, ct);

        user.TotalXp += amount;
        user.CurrentLevel = await RecalculateLevelAsync(user.TotalXp, ct);
        await _userRepository.UpdateAsync(user);

        await _achievementEvaluation.EvaluateForUserAsync(userId, ct);

        return true;
    }
}
