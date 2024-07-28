using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructor.Configurations;

public static class CachingConfiguration
{
    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CacheConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("CacheConnectionString is not configured.");

        // Redis Configuration
        services.AddStackExchangeRedisCache(options => { options.Configuration = connectionString; });

        return services;
    }
}