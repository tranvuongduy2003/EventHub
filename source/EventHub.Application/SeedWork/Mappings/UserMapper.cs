using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserDto>();

        config.CreateMap<User, AuthorDto>();

        config.CreateMap<Pagination<User>, Pagination<UserDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
