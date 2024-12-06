using AutoMapper;
using EventHub.Application.DTOs.Review;
using EventHub.Domain.Aggregates.ReviewAggregate;

namespace EventHub.Application.Mappings;

public sealed class ReviewMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Review, ReviewDto>();
    }
}
