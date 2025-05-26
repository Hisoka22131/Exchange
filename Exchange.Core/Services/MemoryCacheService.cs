using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Exchange.Core.Services;

internal sealed class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key)
    {
        if (!_memoryCache.TryGetValue(key, out T? value))
        {
            return default;
        }
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromDays(1) // TODO: TimeSpan.FromMinutes(15)
                  };

        _memoryCache.Set(key, value, options);
    }

    public void Merge<T>(string key, Func<T, T> update) where T : class, new()
    {
        var existingData = Get<T>(key) ?? new T();
        var updatedData = update(existingData);
        Set(key, updatedData);
    }
    
    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}