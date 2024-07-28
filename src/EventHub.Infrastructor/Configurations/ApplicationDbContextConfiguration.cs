using EventHub.Infrastructor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructor.Configurations;

public static class ApplicationDbContextConfiguration
{
    public static IServiceCollection ConfigureApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("DefaultConnectionString is not configured.");
        services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer(connectionString));
        return services;
    }
}