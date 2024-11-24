using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class CachingConfiguration
{
    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("CacheConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            throw new NullReferenceException("CacheConnectionString is not configured.");
        }

        // Redis Configuration
        services.AddStackExchangeRedisCache(options => { options.Configuration = connectionString; });

        return services;
    }
}
