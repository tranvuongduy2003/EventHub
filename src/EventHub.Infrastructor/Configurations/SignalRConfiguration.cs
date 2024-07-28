using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructor.Configurations;

public static class SignalRConfiguration
{
    public static IServiceCollection ConfigureAzureSignalR(this IServiceCollection services,
        IConfiguration configuration)
    {
        // var azureSignalR = configuration.GetConnectionString("AzureSignalRConnectionString");
        // if (azureSignalR == null || string.IsNullOrEmpty(azureSignalR))
        //     throw new ArgumentNullException("AzureSignalRConnectionString is not configured.");
        services
            .AddSignalR();
            // .AddAzureSignalR(azureSignalR);
        return services;
    }
}