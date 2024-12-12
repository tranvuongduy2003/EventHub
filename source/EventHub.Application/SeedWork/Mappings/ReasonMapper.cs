using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ReasonMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Reason, ReasonDto>();
    }
}
