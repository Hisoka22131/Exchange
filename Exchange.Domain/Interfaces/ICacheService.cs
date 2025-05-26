namespace Exchange.Domain.Interfaces;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    void Remove(string key);
    void Merge<T>(string key, Func<T, T> update) where T : class, new();
}