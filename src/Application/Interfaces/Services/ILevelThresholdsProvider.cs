namespace Application.Interfaces.Services;

public interface ILevelThresholdsProvider
{
    Task<int[]> GetCumulativeThresholdsAsync(CancellationToken ct = default);

    Task InvalidateCacheAsync(CancellationToken ct = default);
}
