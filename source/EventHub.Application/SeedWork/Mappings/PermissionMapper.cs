using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Application.SeedWork.DTOs.Role;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class PermissionMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Role, RoleDto>();

        config.CreateMap<Pagination<Role>, Pagination<RoleDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));

        config
            .CreateMap<Role, RolePermissionDto>()
            .ForMember(dest => dest.Functions, options =>
                options.MapFrom(source => source.Permissions.Select(x => x.Function)));
    }
}
