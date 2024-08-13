using System.Reflection;
using AutoMapper;
using EventHub.Infrastructure.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class MapperConfiguration
{
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        foreach (var property in destinationProperties)
            if (sourceType.GetProperty(property.Name, flags) == null)
                expression.ForMember(property.Name, opt => opt.Ignore());
        return expression;
    }

    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { UserMapper.CreateMap(config); });
        return services;
    }
}