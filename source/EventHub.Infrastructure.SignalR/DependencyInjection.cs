using EventHub.Infrastructure.SignalR.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.SignalR;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureSignalRServices(this IServiceCollection services)
    {
        services.ConfigureAzureSignalR();

        return services;
    }
}
