using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Domain.Aggregates.UserAggregate.Entities;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class FunctionMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<Function, FunctionDto>()
            .ForMember(dest => dest.Level, options =>
                options.MapFrom(source => source.SortOrder));

        config.CreateMap<UpdateFunctionDto, Function>();
    }
}
