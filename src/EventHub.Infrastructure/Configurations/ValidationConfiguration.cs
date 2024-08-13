﻿using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class ValidationConfiguration
{
    public static IServiceCollection ConfigValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        return services;
    }
}