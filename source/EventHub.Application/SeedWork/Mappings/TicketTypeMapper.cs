using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class TicketTypeMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<TicketType, TicketTypeDto>();
    }
}
