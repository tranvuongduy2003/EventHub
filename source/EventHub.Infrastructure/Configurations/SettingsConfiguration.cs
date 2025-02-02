using EventHub.Domain.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class SettingsConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        JwtOptions jwtOptions = configuration.GetSection(nameof(JwtOptions))
            .Get<JwtOptions>();
        services.AddSingleton<JwtOptions>(jwtOptions!);

        MinioStorage minioStorage = configuration.GetSection(nameof(MinioStorage))
            .Get<MinioStorage>();
        services.AddSingleton<MinioStorage>(minioStorage!);

        EmailSettings emailSettings = configuration.GetSection(nameof(EmailSettings))
            .Get<EmailSettings>();
        services.AddSingleton<EmailSettings>(emailSettings!);

        Authentication authentication = configuration.GetSection(nameof(Authentication))
            .Get<Authentication>();
        services.AddSingleton<Authentication>(authentication!);

        HangfireSettings hangfireSettings = configuration.GetSection(nameof(HangfireSettings))
            .Get<HangfireSettings>();
        services.AddSingleton<HangfireSettings>(hangfireSettings!);

        StripeSettings stripeSettings = configuration.GetSection(nameof(StripeSettings))
            .Get<StripeSettings>();
        services.AddSingleton<StripeSettings>(stripeSettings!);

        return services;
    }
}
