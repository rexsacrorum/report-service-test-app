using System;
using Microsoft.Extensions.Caching.Memory;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Services;

public class CacheService<TKeyPrefix> : ICacheService<TKeyPrefix>
{
    private IMemoryCache Cache { get; }
    private readonly TimeSpan _cacheDuration = TimeSpan.FromDays(1);
    
    public CacheService(IMemoryCache cache)
    {
        Cache = cache;
    }
    
    public bool TryGet<T>(string key, out T value)
    {
        return Cache.TryGetValue(GetKey(key), out value);
    }
    
    public void Set<T>(string key, T value)
    {
        Cache.Set(GetKey(key), value, _cacheDuration);
    }
    
    private string GetKey(string key)
    {
        return $"{typeof(TKeyPrefix).Name}_{key}";
    }
}
