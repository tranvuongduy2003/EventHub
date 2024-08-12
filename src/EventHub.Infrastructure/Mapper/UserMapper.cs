using AutoMapper;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructor.Configurations;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Models.User;

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