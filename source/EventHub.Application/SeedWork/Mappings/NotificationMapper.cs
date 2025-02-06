using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class NotificationMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Notification, NotificationDto>().ReverseMap();

        config.CreateMap<Pagination<Notification>, Pagination<NotificationDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
