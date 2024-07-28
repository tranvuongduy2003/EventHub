using EventHub.Infrastructor.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace EventHub.Infrastructor.Configurations;

public static class MiddlewaresConfiguration
{
    public static IApplicationBuilder UseErrorWrapping(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorWrappingMiddleware>();
    }
}