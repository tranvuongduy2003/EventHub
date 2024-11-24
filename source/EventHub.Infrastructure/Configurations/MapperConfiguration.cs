using EventHub.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class MapperConfiguration
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config =>
            {
                UserMapper.CreateMap(config);
                FunctionMapper.CreateMap(config);
                CategoryMapper.CreateMap(config);
                EventMapper.CreateMap(config);
            });
        return services;
    }
}
