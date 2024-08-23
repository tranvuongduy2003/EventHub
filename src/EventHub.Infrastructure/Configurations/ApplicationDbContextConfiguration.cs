using EventHub.Persistence.Data;
using EventHub.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class ApplicationDbContextConfiguration
{
    public static IServiceCollection ConfigureApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("DefaultConnectionString is not configured.");
        services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
            {
                var inteceptor = provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                optionsBuilder
                    .UseSqlServer(connectionString)
                    .AddInterceptors(inteceptor);
            });
        return services;
    }
}