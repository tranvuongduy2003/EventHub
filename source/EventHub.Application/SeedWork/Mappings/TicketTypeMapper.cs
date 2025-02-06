using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class TicketTypeMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<TicketType, TicketTypeDto>();

        config.CreateMap<Pagination<TicketType>, Pagination<TicketTypeDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
