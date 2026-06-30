using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Thoitiet.Caching;

public class RedisCacheService
    : ICacheService
{
    private readonly IDistributedCache
        _cache;

    private static readonly SemaphoreSlim
        _lock = new(1, 1);

    public RedisCacheService(
        IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?>
        GetAsync<T>(
        string key)
    {
        var json =
            await _cache
            .GetStringAsync(key);

        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer
            .Deserialize<T>(json);
    }

    public async Task
        SetAsync<T>(

        string key,

        T value,

        TimeSpan ttl

    )
    {
        var json =

            JsonSerializer
            .Serialize(value);

        await _cache
            .SetStringAsync(

            key,

            json,

            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow
                    = ttl
            }

        );
    }

    public async Task<T>
        GetOrCreateAsync<T>(
        string key,

        Func<Task<T>> factory,

        TimeSpan ttl)
    {
        var cache =
            await GetAsync<T>(
            key);

        if (cache != null)
        {
            return cache;
        }

        await _lock.WaitAsync();

        try
        {
            cache =
                await GetAsync<T>(
                key);

            if (cache != null)
            {
                return cache;
            }

            var value =
                await factory();

            await _cache
                .SetStringAsync(

                key,

                JsonSerializer
                .Serialize(value),

                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow
                        = ttl
                });

            return value;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task RemoveAsync(
        string key)
    {
        await _cache
            .RemoveAsync(key);
    }
}