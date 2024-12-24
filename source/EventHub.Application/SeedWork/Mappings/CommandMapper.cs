using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Command;
using EventHub.Domain.Aggregates.UserAggregate.Entities;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class CommandMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Command, CommandDto>();
    }
}

