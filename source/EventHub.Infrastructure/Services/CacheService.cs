using EventHub.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace EventHub.Infrastructure.Caching;

/// <summary>
/// Provides an implementation of a caching service using a distributed cache and serialization.
/// </summary>
/// <remarks>
/// This class manages cache operations, including storing and retrieving data from a distributed cache.
/// It uses a serialization service to handle data conversion and a _logger to record cache operations.
/// </remarks>
public class CacheService : ICacheService
{
    private readonly ILogger _logger;
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="redisCacheService">The distributed cache service used to store and retrieve cache data.</param>
    /// <param name="serializeService">The service used for serializing and deserializing cache data.</param>
    /// <param name="logger">The _logger used to log cache operations and errors.</param>
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
        _logger.Information("BEGIN: GetData<{DataType}>(key: {KeyName})", typeof(T).Name, key);

        string value = await _redisCacheService.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            return _serializeService.Deserialize<T>(value);
        }

        _logger.Information("END: GetData<{DataType}>", typeof(T).Name);

        return default;
    }

    public async Task<bool> SetData<T>(string key, T value, TimeSpan? expirationTime)
    {
        _logger.Information("BEGIN: GetData<{DataType}>(key: {KeyName}, value: {Value})", typeof(T).Name, key, value);

        var options = new DistributedCacheEntryOptions();

        if (expirationTime.HasValue)
        {
            options.SetAbsoluteExpiration(expirationTime.Value);
        }

        await _redisCacheService.SetStringAsync(key, _serializeService.Serialize(value), options);

        _logger.Information("END: GetData<{DataType}>", typeof(T).Name);

        return true;
    }

    public async Task<bool> RemoveData(string key)
    {
        try
        {
            _logger.Information("BEGIN: RemoveData(key: {KeyName})", key);

            string value = await _redisCacheService.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Value is null or empty!");
            }

            await _redisCacheService.RemoveAsync(key);

            _logger.Information("END: RemoveData");

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "RemoveData: {Message}", ex.Message);
            return false;
        }
    }
}
