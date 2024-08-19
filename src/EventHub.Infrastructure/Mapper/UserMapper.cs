using AutoMapper;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructure.Configurations;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Models.User;

namespace EventHub.Infrastructure.Mapper;

public class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserModel>().ReverseMap();

        config.CreateMap<UserModel, UserDto>().IgnoreAllNonExisting();

        config
            .CreateMap<CreateUserDto, UserModel>()
            .ForMember(dest => dest.Avatar, options => options.Ignore());

        config
            .CreateMap<UpdateUserDto, UserModel>()
            .ForMember(dest => dest.Avatar, options => options.Ignore());
    }
}