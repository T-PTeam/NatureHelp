using Application.Interfaces.Services.Cache;

namespace Application.Services.Cache;

public class DictValueCacheService : IDictValueCacheService
{
    private const string KeyPrefix = "dict:";

    private readonly IRedisCacheService _redis;

    public DictValueCacheService(IRedisCacheService redis)
    {
        _redis = redis;
    }

    private static string CacheKey(string entryKey) => KeyPrefix + entryKey;

    public Task<string?> GetValueJsonAsync(string entryKey) =>
        _redis.GetAsync(CacheKey(entryKey));

    public Task SetValueJsonAsync(string entryKey, string valueJson, TimeSpan? expiry = null) =>
        _redis.SetAsync(CacheKey(entryKey), valueJson, expiry);

    public Task RemoveAsync(string entryKey) =>
        _redis.RemoveAsync(CacheKey(entryKey));
}
