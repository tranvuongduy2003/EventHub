using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Aggregates.UserAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserDto>();

        config.CreateMap<User, AuthorDto>();
    }
}
