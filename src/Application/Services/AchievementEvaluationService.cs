using Application.Interfaces.Services;
using Domain.Enums;
using Domain.Models.Profile;
using Infrastructure.Interfaces;

namespace Application.Services;

public class AchievementEvaluationService : IAchievementEvaluationService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILevelThresholdsProvider _levelThresholdsProvider;

    public AchievementEvaluationService(
        IProfileRepository profileRepository,
        IUserRepository userRepository,
        ILevelThresholdsProvider levelThresholdsProvider)
    {
        _profileRepository = profileRepository;
        _userRepository = userRepository;
        _levelThresholdsProvider = levelThresholdsProvider;
    }

    public async Task EvaluateForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return;

        var water = await _profileRepository.CountWaterByCreatorAsync(userId, ct);
        var soil = await _profileRepository.CountSoilByCreatorAsync(userId, ct);
        var totalReports = water + soil;
        var confirmationsGiven = await _profileRepository.CountConfirmationsGivenByUserAsync(userId, ct);
        var hasFiveOnOwn = await _profileRepository.CreatorHasDeficiencyWithFiveConfirmationsAsync(userId, ct);

        var achievements = await _profileRepository.GetActiveAchievementsAsync(ct);
        var thresholds = await _levelThresholdsProvider.GetCumulativeThresholdsAsync(ct);
        var levelFromXp = LevelProgressCalculator.ComputeLevel(user.TotalXp, thresholds);

        foreach (var ach in achievements)
        {
            var target = ach.TargetInt ?? 0;
            var actual = ach.RuleType switch
            {
                AchievementRuleType.TotalReportsCount => totalReports,
                AchievementRuleType.WaterReportsCount => water,
                AchievementRuleType.SoilReportsCount => soil,
                AchievementRuleType.TotalXp => user.TotalXp,
                AchievementRuleType.CurrentLevel => levelFromXp,
                AchievementRuleType.ConfirmationsGivenCount => confirmationsGiven,
                AchievementRuleType.DeficiencyGotFiveConfirmations => hasFiveOnOwn ? 1 : 0,
                _ => 0,
            };

            var completed = target > 0 ? actual >= target : actual > 0;
            var progress = target > 0 ? Math.Min(actual, target) : (completed ? 1 : 0);

            var existing = await _profileRepository.GetUserAchievementAsync(userId, ach.Id, ct);
            var unlockedAt = existing?.UnlockedAt;
            if (completed && unlockedAt == null)
                unlockedAt = DateTime.UtcNow;
            else if (!completed)
                unlockedAt = null;

            await _profileRepository.UpsertUserAchievementAsync(new UserAchievement
            {
                UserId = userId,
                AchievementId = ach.Id,
                Progress = progress,
                IsCompleted = completed,
                UnlockedAt = unlockedAt,
            }, ct);
        }
    }
}
