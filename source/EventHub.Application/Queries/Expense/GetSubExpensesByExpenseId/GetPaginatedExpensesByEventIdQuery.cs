using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Expense.GetSubExpensesByExpenseId;

public record GetSubExpensesByExpenseIdQuery(Guid ExpenseId) : IQuery<List<SubExpenseDto>>;
