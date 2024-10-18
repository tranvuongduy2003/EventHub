using AutoMapper;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Shared.DTOs.Review;

namespace EventHub.Application.Mappings;

public class ReviewMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Review, ReviewDto>();
    }
}