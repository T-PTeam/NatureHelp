﻿namespace Application.Interfaces.Services.Cache;
public interface IRedisCacheService
{
    Task SetAsync(string key, string value, TimeSpan? expiry = null);

    Task<string?> GetAsync(string key);

    Task RemoveAsync(string key);
}
