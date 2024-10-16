using EventHub.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}