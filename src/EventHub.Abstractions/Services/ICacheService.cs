namespace EventHub.Abstractions.Services;

/// <summary>
/// Defines the contract for a caching service that provides methods to interact with a cache.
/// </summary>
/// <remarks>
/// This interface outlines the basic operations for a caching service, including retrieving, setting, and removing cached data.
/// It is designed to support various types of data and allows specifying an expiration time for cached entries.
/// </remarks>
public interface ICacheService
{
    /// <summary>
    /// Retrieves data from the cache.
    /// </summary>
    /// <typeparam name="T">The type of the data to retrieve.</typeparam>
    /// <param name="key">The key associated with the cached data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached data of type <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// If the data is not found in the cache, this method should return the default value of <typeparamref name="T"/>.
    /// </remarks>
    Task<T> GetData<T>(string key);

    /// <summary>
    /// Stores data in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the data to store.</typeparam>
    /// <param name="key">The key associated with the data to cache.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="expirationTime">The optional expiration time for the cached data. If not provided, the data will not expire.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the data was successfully stored in the cache.</returns>
    /// <remarks>
    /// If the expiration time is provided, the cached data will expire after the specified duration.
    /// </remarks>
    Task<bool> SetData<T>(string key, T value, TimeSpan? expirationTime);

    /// <summary>
    /// Removes data from the cache.
    /// </summary>
    /// <param name="key">The key associated with the data to remove from the cache.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the removed data, or null if the key was not found.</returns>
    /// <remarks>
    /// This method removes the data associated with the specified key from the cache.
    /// If the key does not exist in the cache, this method will return null.
    /// </remarks>
    Task<object> RemoveData(string key);
}