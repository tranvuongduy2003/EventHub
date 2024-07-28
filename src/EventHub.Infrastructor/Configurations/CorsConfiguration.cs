using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructor.Configurations;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string appCors)
    {
        services.AddCors(p =>
            p.AddPolicy(appCors, build =>
            {
                build
                    .WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

        return services;
    }
}