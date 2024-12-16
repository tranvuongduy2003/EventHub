using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Domain.Aggregates.UserAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class PermissionMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config
            .CreateMap<Role, RolePermissionDto>()
            .ForMember(dest => dest.Functions, options =>
                options.MapFrom(source => source.Permissions.Select(x => x.Function)));
    }
}
