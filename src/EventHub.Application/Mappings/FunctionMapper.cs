using AutoMapper;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Shared.DTOs.Function;

namespace EventHub.Application.Mappings;

public class FunctionMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config
            .CreateMap<Function, FunctionDto>()
            .ForMember(dest => dest.ParentId, options => options.Ignore())
            .ForMember(dest => dest.SortOrder, options => options.Ignore())
            .ForMember(dest => dest.Url, options => options.Ignore());

        config.CreateMap<UpdateFunctionDto, Function>();
    }
}