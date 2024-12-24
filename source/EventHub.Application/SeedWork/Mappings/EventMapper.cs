using AutoMapper;
using EventHub.Application.Commands.Event.CreateEvent;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class EventMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Event, EventDto>()
            .ForMember(dest => dest.Categories, options =>
                options.MapFrom(source => source.EventCategories.Select(x => x.Category)))
            .ForMember(dest => dest.Coupons, options =>
                options.MapFrom(source => source.EventCoupons.Select(x => x.Coupon)));

        config.CreateMap<Pagination<Event>, Pagination<EventDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
        
        config.CreateMap<Event, ReviewedEventDto>();

        config.CreateMap<Event, EventDetailDto>()
            .ForMember(dest => dest.EventSubImageUrls, options =>
                options.MapFrom(source => source.EventSubImages.Select(image => image.ImageUrl)))
            .ForMember(dest => dest.Reasons, options =>
                options.MapFrom(source => source.Reasons.Select(reason => reason.Name)))
            .ForMember(dest => dest.Categories, options =>
                options.MapFrom(source => source.EventCategories.Select(category => category.Category)))
            .ForMember(dest => dest.Coupons, options =>
                options.MapFrom(source => source.EventCoupons.Select(x => x.Coupon)));

        config.CreateMap<CreateEventCommand, CreateEventDto>().ReverseMap();

        config.CreateMap<CreateEventCommand, Event>()
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
