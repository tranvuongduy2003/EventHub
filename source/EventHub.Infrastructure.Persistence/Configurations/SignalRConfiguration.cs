using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.SignalR.Configurations;

public static class SignalRConfiguration
{
    public static IServiceCollection ConfigureAzureSignalR(this IServiceCollection services)
    {
        services
            .AddSignalR();
        return services;
    }
}
