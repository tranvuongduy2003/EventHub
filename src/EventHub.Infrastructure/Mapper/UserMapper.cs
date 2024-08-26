using AutoMapper;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Shared.DTOs.User;

namespace EventHub.Infrastructure.Mapper;

public class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserDto>();
    }
}