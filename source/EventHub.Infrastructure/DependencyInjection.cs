using EventHub.Application.SeedWork.Abstractions;
using EventHub.Infrastructure.Caching;
using EventHub.Infrastructure.Clock;
using EventHub.Infrastructure.Configurations;
using EventHub.Infrastructure.FilesSystem;
using EventHub.Infrastructure.Mailler;
using EventHub.Infrastructure.Persistence;
using EventHub.Infrastructure.Services;
using EventHub.Infrastructure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, string appCors)
    {
        services.ConfigureRedis(configuration);
        services.ConfigureControllers();
        services.ConfigureCors(appCors);
        services.ConfigureApplicationDbContext(configuration);
        services.ConfigureQuartz();
        services.ConfigureHangfireServices();
        services.ConfigureMediatR();
        services.ConfigureIdentity();
        services.ConfigureMapper();
        services.ConfigValidation();
        services.ConfigureSwagger();
        services.ConfigureAppSettings(configuration);
        services.ConfigureApplication();
        services.ConfigureAuthetication();
        services.ConfigureMinioStorage();
        services.ConfigureDependencyInjection();
        services.ConfigurePersistenceServices();
        services.ConfigureSignalRServices();
        services.ConfigureHttpClients(configuration);

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddSingleton<IFileService, MinioFileService>();

        services
            .AddTransient<ISerializeService, SerializeService>()
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<IHangfireService, HangfireService>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
