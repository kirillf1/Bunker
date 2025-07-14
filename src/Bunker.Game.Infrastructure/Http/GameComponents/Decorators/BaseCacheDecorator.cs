using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Decorators;

public abstract class BaseCacheDecorator
{
    protected readonly IDistributedCache _cache;
    protected readonly GameComponentsClientOptions _options;

    protected BaseCacheDecorator(
        IDistributedCache cache,
        IOptions<GameComponentsClientOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    protected DistributedCacheEntryOptions GetCacheOptions()
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationTimeInMinutes)
        };
    }

    protected async Task<T> GetFromCacheOrSource<T>(string cacheKey, Func<Task<T>> source)
    {
        if (!_options.CacheEnabled)
        {
            return await source();
        }

        var cachedValue = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedValue))
        {
            return JsonSerializer.Deserialize<T>(cachedValue)!;
        }

        var result = await source();
        var serializedResult = JsonSerializer.Serialize(result);
        await _cache.SetStringAsync(cacheKey, serializedResult, GetCacheOptions());

        return result;
    }

    protected async Task<ICollection<T>> GetCollectionFromCacheOrSource<T>(
        string collectionCacheKey,
        Func<Task<ICollection<T>>> source)
    {
        if (!_options.CacheEnabled)
        {
            return await source();
        }

        // Пытаемся получить коллекцию из кэша
        var cachedCollection = await _cache.GetStringAsync(collectionCacheKey);
        if (!string.IsNullOrEmpty(cachedCollection))
        {
            return JsonSerializer.Deserialize<ICollection<T>>(cachedCollection)!;
        }

        // Если коллекции нет в кэше, получаем из источника
        var result = await source();
        var cacheOptions = GetCacheOptions();

        // Кэшируем только коллекцию целиком
        var serializedCollection = JsonSerializer.Serialize(result);
        await _cache.SetStringAsync(collectionCacheKey, serializedCollection, cacheOptions);

        return result;
    }
} 