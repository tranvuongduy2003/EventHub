using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Domain.Aggregates.CategoryAggregate;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
    }
}
