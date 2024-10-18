using AutoMapper;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.Mappings;

public class EventMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Event, EventDto>();

        config.CreateMap<Event, EventDetailDto>()
            .ForMember(dest => dest.EventSubImageUrls, options =>
                options.MapFrom(source =>
                    source.EventSubImages.Select(image => image.ImageUrl)));

        config.CreateMap<CreateEventDto, Event>()
            .ForMember(dest => dest.CoverImageUrl, options => options.Ignore())
            .ForMember(dest => dest.CoverImageFileName, options => options.Ignore())
            .ForMember(dest => dest.Status, options => options.Ignore())
            .ForMember(dest => dest.AuthorId, options => options.Ignore())
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.TicketTypes, options => options.Ignore())
            .ForMember(dest => dest.Reasons, options => options.Ignore())
            .ForMember(dest => dest.EventSubImages, options => options.Ignore())
            .ForMember(dest => dest.EmailContent, options => options.Ignore());
    }
}