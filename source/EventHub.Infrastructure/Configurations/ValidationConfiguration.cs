using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class ValidationConfiguration
{
    public static IServiceCollection ConfigValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly)
            .AddFluentValidationRulesToSwagger();

        return services;
    }
}
