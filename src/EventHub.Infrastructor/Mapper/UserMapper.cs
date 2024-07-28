using AutoMapper;
using EventHub.Domain.Common.Entities;
using EventHub.Domain.DTOs.User;
using EventHub.Domain.Models.User;
using EventHub.Infrastructor.Configurations;

namespace EventHub.Infrastructor.Mapper;

public class UserMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<User, UserModel>().IgnoreAllNonExisting();

        config.CreateMap<UserModel, User>().IgnoreAllNonExisting();

        config.CreateMap<CreateUserDto, User>().IgnoreAllNonExisting();

        config.CreateMap<UpdateUserDto, User>().IgnoreAllNonExisting();
    }
}