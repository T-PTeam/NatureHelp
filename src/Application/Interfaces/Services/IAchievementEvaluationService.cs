namespace Application.Interfaces.Services;

public interface IAchievementEvaluationService
{
    Task EvaluateForUserAsync(Guid userId, CancellationToken ct = default);
}
