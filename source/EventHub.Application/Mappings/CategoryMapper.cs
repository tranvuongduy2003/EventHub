using AutoMapper;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Application.Mappings;

public sealed class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
    }
}
