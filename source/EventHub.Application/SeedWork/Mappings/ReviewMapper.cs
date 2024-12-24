using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ReviewMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Review, ReviewDto>();
        
        config.CreateMap<Pagination<Review>, Pagination<ReviewDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
