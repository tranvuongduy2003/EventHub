using AutoMapper;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Shared.DTOs.Command;

namespace EventHub.Application.Mappings;

public sealed class CommandMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Command, CommandDto>();
    }
}

