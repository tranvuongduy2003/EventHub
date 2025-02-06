using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ExpenseMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<SubExpense, SubExpenseDto>();

        config.CreateMap<Expense, ExpenseDto>();

        config.CreateMap<Pagination<Expense>, Pagination<ExpenseDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));

        config.CreateMap<Pagination<SubExpense>, Pagination<SubExpenseDto>>()
            .ForMember(dest => dest.Items, options =>
                options.MapFrom(source => source.Items));
    }
}
