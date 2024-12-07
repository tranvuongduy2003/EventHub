using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.Aggregates.ReviewAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ReviewMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Review, ReviewDto>();
    }
}
