namespace ReportService.Application.Interfaces;

public interface ICacheService<TKeyPrefix>
{
    /// <summary>
    /// Try to get value from cache.
    /// </summary>
    bool TryGet<T>(string key, out T value);
    
    /// <summary>
    /// Set value to cache.
    /// </summary>
    void Set<T>(string key, T value);
}