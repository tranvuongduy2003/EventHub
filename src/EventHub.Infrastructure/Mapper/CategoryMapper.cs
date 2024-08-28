using AutoMapper;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Infrastructure.Mapper;

public class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
    }
}