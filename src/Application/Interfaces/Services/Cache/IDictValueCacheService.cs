namespace Application.Interfaces.Services.Cache;

public interface IDictValueCacheService
{
    Task<string?> GetValueJsonAsync(string entryKey);

    Task SetValueJsonAsync(string entryKey, string valueJson, TimeSpan? expiry = null);

    Task RemoveAsync(string entryKey);
}
