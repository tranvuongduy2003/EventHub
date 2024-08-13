using EventHub.Domain.Services;
using EventHub.Infrastructure.Caching;
using EventHub.Infrastructure.Configurations;
using EventHub.Infrastructure.FilesSystem;
using EventHub.Infrastructure.Hangfire;
using EventHub.Infrastructure.Mailler;
using EventHub.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHangfireService = EventHub.Domain.Services.IHangfireService;

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
        services.ConfigureHangfireServices();
        services.ConfigureMediatR();
        services.ConfigureIdentity();
        services.ConfigureMapper();
        services.ConfigValidation();
        services.ConfigureSwagger();
        services.ConfigureAppSettings(configuration);
        services.ConfigureApplication();
        services.ConfigureAuthetication();
        services.ConfigureDependencyInjection();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddTransient<ISerializeService, SerializeService>()
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<IBlobService, AzureBlobService>()
            .AddTransient<IHangfireService, HangfireService>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<ITokenService, TokenService>();

        return services;
    }
}