using AutoMapper;
using EventHub.Application.DTOs.Command;
using EventHub.Domain.Aggregates.PermissionAggregate;

namespace EventHub.Application.Mappings;

public sealed class CommandMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Command, CommandDto>();
    }
}

