using EventHub.Application.SeedWork.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class MapperConfiguration
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config =>
            {
                CategoryMapper.CreateMap(config);
                CommandMapper.CreateMap(config);
                EventMapper.CreateMap(config);
                FunctionMapper.CreateMap(config);
                ReviewMapper.CreateMap(config);
                UserMapper.CreateMap(config);
            });
        return services;
    }
}
