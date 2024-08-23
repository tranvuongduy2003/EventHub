using EventHub.Domain.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace EventHub.Infrastructure.Caching;

/// <summary>
/// Provides an implementation of a caching service using a distributed cache and serialization.
/// </summary>
/// <remarks>
/// This class manages cache operations, including storing and retrieving data from a distributed cache.
/// It uses a serialization service to handle data conversion and a logger to record cache operations.
/// </remarks>
public class CacheService : ICacheService
{
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="redisCacheService">The distributed cache service used to store and retrieve cache data.</param>
    /// <param name="serializeService">The service used for serializing and deserializing cache data.</param>
    /// <param name="logger">The logger used to log cache operations and errors.</param>
    /// <remarks>
    /// The constructor sets up the caching service with the necessary components for cache management:
    /// - <paramref name="redisCacheService"/>: Handles interactions with the distributed cache.
    /// - <paramref name="serializeService"/>: Manages data serialization and deserialization.
    /// - <paramref name="logger"/>: Provides logging capabilities for monitoring and debugging cache operations.
    /// </remarks>
    public CacheService(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger)
    {
        _redisCacheService = redisCacheService;
        _serializeService = serializeService;
        _logger = logger;
    }

    public async Task<T> GetData<T>(string key)
    {
        _logger.Information($"BEGIN: GetData<{nameof(T)}>(key: {key})");

        var value = await _redisCacheService.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
            return _serializeService.Deserialize<T>(value);

        _logger.Information($"END: GetData<{nameof(T)}>");

        return default;
    }

    public async Task<bool> SetData<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        _logger.Information($"BEGIN: GetData<{nameof(T)}>(key: {key}, value: {value})");

        var options = new DistributedCacheEntryOptions();
        if (expirationTime.HasValue)
            options.SetAbsoluteExpiration(expirationTime.Value);

        await _redisCacheService.SetStringAsync(key, _serializeService.Serialize(value), options);

        _logger.Information($"BEGIN: GetData<{nameof(T)}>");

        return true;
    }

    public async Task<object> RemoveData(string key)
    {
        try
        {
            _logger.Information($"BEGIN: RemoveData(key: {key})");

            var value = await _redisCacheService.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                throw new Exception("Value is null or empty!");

            await _redisCacheService.RemoveAsync(key);

            _logger.Information($"END: RemoveData");

            return true;
        }
        catch (Exception e)
        {
            _logger.Error("RemoveData: " + e.Message);
            throw;
        }
    }
}