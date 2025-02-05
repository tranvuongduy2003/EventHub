using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Expense.GetExpenseById;

/// <summary>
/// Represents a query to retrieve a expense's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a expense's information based on its unique identifier.
/// </remarks>
public record GetExpenseByIdQuery(Guid ExpenseId) : IQuery<ExpenseDto>;
