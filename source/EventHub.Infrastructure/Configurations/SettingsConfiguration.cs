using EventHub.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class SettingsConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions))
            .Get<JwtOptions>();
        services.AddSingleton<JwtOptions>(jwtOptions);

        var azureBlobStorage = configuration.GetSection(nameof(AzureBlobStorage))
            .Get<AzureBlobStorage>();
        services.AddSingleton<AzureBlobStorage>(azureBlobStorage);

        var emailSettings = configuration.GetSection(nameof(EmailSettings))
            .Get<EmailSettings>();
        services.AddSingleton<EmailSettings>(emailSettings);

        var authentication = configuration.GetSection(nameof(Authentication))
            .Get<Authentication>();
        services.AddSingleton<Authentication>(authentication);

        var hangfireSettings = configuration.GetSection(nameof(HangfireSettings))
            .Get<HangfireSettings>();
        services.AddSingleton<HangfireSettings>(hangfireSettings);

        return services;
    }
}