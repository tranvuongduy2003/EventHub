using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.Aggregates.TicketAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class TicketMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Ticket, TicketDto>();
    }
}
