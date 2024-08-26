using AutoMapper;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Infrastructure.Mapper;

public class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>();

        config.CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.IconImageUrl, options => options.Ignore())
            .ForMember(dest => dest.IconImageFileName, options => options.Ignore());
    }
}