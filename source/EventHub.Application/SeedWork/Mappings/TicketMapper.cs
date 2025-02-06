using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class TicketMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Ticket, TicketDto>();

        config.CreateMap<Pagination<Ticket>, Pagination<TicketDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
