using AutoMapper;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Models.Category;

namespace EventHub.Infrastructure.Mapper;

public class CategoryMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Category, CategoryModel>().ReverseMap();

        config.CreateMap<CategoryModel, CategoryDto>();

        config
            .CreateMap<CreateCategoryDto, CategoryModel>()
            .ForMember(dest => dest.IconImage, options => options.Ignore());

        config
            .CreateMap<UpdateCategoryDto, CategoryModel>()
            .ForMember(dest => dest.IconImage, options => options.Ignore());
    }
}