using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.Aggregates.NotificationAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class NotificationMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Notification, NotificationDto>().ReverseMap();
    }
}
