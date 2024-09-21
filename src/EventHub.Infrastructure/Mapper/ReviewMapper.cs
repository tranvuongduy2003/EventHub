using AutoMapper;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Shared.DTOs.Review;

namespace EventHub.Infrastructure.Mapper;

public class ReviewMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Review, ReviewDto>();
    }
}