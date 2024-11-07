using EventHub.Persistence.Data;
using EventHub.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EventHub.Infrastructure.Configurations;

public static class ApplicationDbContextConfiguration
{
    public static IServiceCollection ConfigureApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<DateTrackingInterceptor>();

        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("DefaultConnectionString is not configured.");
        services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
        {
            var convertDomainEventsToOutboxMessagesInterceptor =
                provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
            var dateTrackingInterceptor =
                provider.GetService<DateTrackingInterceptor>()!;

            optionsBuilder
                .UseSqlServer(connectionString, builder =>
                    builder.MigrationsAssembly("EventHub.Persistence"))
                .AddInterceptors(
                    dateTrackingInterceptor,
                    convertDomainEventsToOutboxMessagesInterceptor
                );
        });
        return services;
    }
}