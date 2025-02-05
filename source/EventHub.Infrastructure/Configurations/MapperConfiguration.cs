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
                CouponMapper.CreateMap(config);
                ReasonMapper.CreateMap(config);
                EmailContentMapper.CreateMap(config);
                EventMapper.CreateMap(config);
                ExpenseMapper.CreateMap(config);
                FunctionMapper.CreateMap(config);
                PaymentMapper.CreateMap(config);
                PermissionMapper.CreateMap(config);
                ReasonMapper.CreateMap(config);
                ReviewMapper.CreateMap(config);
                TicketMapper.CreateMap(config);
                TicketTypeMapper.CreateMap(config);
                UserMapper.CreateMap(config);
            });
        return services;
    }
}
