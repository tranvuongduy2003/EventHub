using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();
        
        config.CreateMap<Pagination<Category>, Pagination<CategoryDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
