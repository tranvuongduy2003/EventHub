using EventHub.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace EventHub.Infrastructor.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;

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