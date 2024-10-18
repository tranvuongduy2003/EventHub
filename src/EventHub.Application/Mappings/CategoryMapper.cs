using AutoMapper;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Application.Mappings;

public class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
    }
}