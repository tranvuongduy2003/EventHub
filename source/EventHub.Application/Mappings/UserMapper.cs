using AutoMapper;
using EventHub.Application.DTOs.User;
using EventHub.Domain.Aggregates.UserAggregate;

namespace EventHub.Application.Mappings;

public sealed class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserDto>();
    }
}
