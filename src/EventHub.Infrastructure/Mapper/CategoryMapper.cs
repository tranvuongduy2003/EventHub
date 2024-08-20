using AutoMapper;
using EventHub.Application.Commands.Category.CreateCategory;
using EventHub.Application.Commands.Category.UpdateCategory;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Infrastructure.Configurations;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Infrastructure.Mapper;

public class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryDto>()
            .IgnoreAllNonExisting();
        
        config.CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.IconImage, options => options.Ignore());
    }
}