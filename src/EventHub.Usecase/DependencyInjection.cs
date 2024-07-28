using EventHub.Domain.Usecases;
using EventHub.Usecase.Usecases;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Usecase;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureUsecaseServices(this IServiceCollection services)
    {
        services.ConfigureDependencyInjection();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddTransient<IAuthUsecase, AuthUsecase>();

        return services;
    }
}