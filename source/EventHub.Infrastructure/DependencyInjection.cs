using EventHub.Abstractions;
using EventHub.Abstractions.Services;
using EventHub.Application.Idempotence;
using EventHub.Infrastructure.Caching;
using EventHub.Infrastructure.Clock;
using EventHub.Infrastructure.Configurations;
using EventHub.Infrastructure.FilesSystem;
using EventHub.Infrastructure.Mailler;
using EventHub.Infrastructure.Outbox;
using EventHub.Infrastructure.Services;
using MediatR;
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
        services.ConfigureDependencyInjection();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();

        services
            .AddTransient<ISerializeService, SerializeService>()
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<IFileService, AzureFileService>()
            .AddTransient<IHangfireService, HangfireService>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}