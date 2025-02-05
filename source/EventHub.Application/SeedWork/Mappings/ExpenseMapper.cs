using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;

namespace EventHub.Application.SeedWork.Mappings;

public sealed class ExpenseMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<SubExpense, SubExpenseDto>();

        config.CreateMap<Expense, ExpenseDto>();
    }
}
