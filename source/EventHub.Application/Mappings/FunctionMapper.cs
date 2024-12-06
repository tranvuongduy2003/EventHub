using AutoMapper;
using EventHub.Application.DTOs.Function;
using EventHub.Domain.Aggregates.PermissionAggregate;

namespace EventHub.Application.Mappings;

public sealed class FunctionMapper
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
