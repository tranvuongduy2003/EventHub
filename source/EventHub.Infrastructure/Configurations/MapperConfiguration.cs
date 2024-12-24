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
                ReasonMapper.CreateMap(config);
                TicketTypeMapper.CreateMap(config);
                EmailContentMapper.CreateMap(config);
                CategoryMapper.CreateMap(config);
                CommandMapper.CreateMap(config);
                EventMapper.CreateMap(config);
                FunctionMapper.CreateMap(config);
                ReviewMapper.CreateMap(config);
                UserMapper.CreateMap(config);
                PermissionMapper.CreateMap(config);
                CouponMapper.CreateMap(config);
            });
        return services;
    }
}
