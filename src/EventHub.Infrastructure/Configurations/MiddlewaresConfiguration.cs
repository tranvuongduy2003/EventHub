using EventHub.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace EventHub.Infrastructure.Configurations;

public static class MiddlewaresConfiguration
{
    public static IApplicationBuilder UseErrorWrapping(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorWrappingMiddleware>();
    }
}