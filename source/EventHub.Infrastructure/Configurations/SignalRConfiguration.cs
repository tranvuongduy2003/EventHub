using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class SignalRConfiguration
{
    public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
    {
        services
            .AddSignalR();
        return services;
    }
}
