using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Invitation;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, options =>
                options.MapFrom(source => (source.Roles ?? new List<Role>()).Select(x => x.Name)))
            .ForMember(dest => dest.Avatar, options =>
                options.MapFrom(source => source.AvatarUrl));

        config.CreateMap<User, AuthorDto>()
            .ForMember(dest => dest.Avatar, options =>
                options.MapFrom(source => source.AvatarUrl));

        config.CreateMap<Pagination<User>, Pagination<UserDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));

        config.CreateMap<Invitation, InvitationDto>();

        config.CreateMap<Pagination<Invitation>, Pagination<InvitationDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
