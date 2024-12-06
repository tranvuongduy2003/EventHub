using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.Interceptors;
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
        services.AddSingleton<DateTrackingInterceptor>();

        string connectionString = configuration.GetConnectionString("DefaultConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            throw new NullReferenceException("DefaultConnectionString is not configured.");
        }
        services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
        {
            ConvertDomainEventsToOutboxMessagesInterceptor convertDomainEventsToOutboxMessagesInterceptor =
                provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
            DateTrackingInterceptor dateTrackingInterceptor =
                provider.GetService<DateTrackingInterceptor>()!;

            optionsBuilder
                .UseSqlServer(connectionString, builder =>
                    builder.MigrationsAssembly("EventHub.Infrastructure.Persistence"))
                .AddInterceptors(
                    dateTrackingInterceptor,
                    convertDomainEventsToOutboxMessagesInterceptor
                );
        });
        return services;
    }
}
