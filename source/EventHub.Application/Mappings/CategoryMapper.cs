using AutoMapper;
using EventHub.Application.DTOs.Category;
using EventHub.Domain.Aggregates.CategoryAggregate;

namespace EventHub.Application.Mappings;

public sealed class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
    }
}
