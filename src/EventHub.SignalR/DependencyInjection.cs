using EventHub.SignalR.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.SignalR;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureSignalRServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureAzureSignalR(configuration);
        
        return services;
    }
}