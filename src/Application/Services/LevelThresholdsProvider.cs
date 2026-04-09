using System.Text.Json;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Cache;
using Infrastructure.Interfaces;

namespace Application.Services;

public class LevelThresholdsProvider : ILevelThresholdsProvider
{
    public const string DictEntryKey = "level_thresholds";

    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(24);

    private readonly IDictValueCacheService _dictValueCache;
    private readonly IDictEntryRepository _dictEntryRepository;

    public LevelThresholdsProvider(IDictValueCacheService dictValueCache, IDictEntryRepository dictEntryRepository)
    {
        _dictValueCache = dictValueCache;
        _dictEntryRepository = dictEntryRepository;
    }

    public async Task<int[]> GetCumulativeThresholdsAsync(CancellationToken ct = default)
    {
        var cachedJson = await _dictValueCache.GetValueJsonAsync(DictEntryKey);
        if (!string.IsNullOrEmpty(cachedJson))
            return DeserializeOrThrow(cachedJson);

        var json = await _dictEntryRepository.GetValueJsonByKeyAsync(DictEntryKey, ct);
        if (string.IsNullOrWhiteSpace(json))
            throw new InvalidOperationException($"Missing dict entry '{DictEntryKey}'.");

        var thresholds = DeserializeOrThrow(json);
        await _dictValueCache.SetValueJsonAsync(DictEntryKey, json, CacheTtl);
        return thresholds;
    }

    public Task InvalidateCacheAsync(CancellationToken ct = default) =>
        _dictValueCache.RemoveAsync(DictEntryKey);

    private static int[] DeserializeOrThrow(string json)
    {
        var arr = JsonSerializer.Deserialize<int[]>(json);
        if (arr == null || arr.Length == 0)
            throw new InvalidOperationException($"Invalid JSON for '{DictEntryKey}': expected non-empty int array.");
        return arr;
    }
}
