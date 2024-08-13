using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Persistence.Data;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenProviders.GOOGLE)
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenProviders.FACEBOOK)
            .AddDefaultTokenProviders();

        return services;
    }
}