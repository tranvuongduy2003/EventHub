using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Expense.GetPaginatedExpenses;

/// <summary>
/// Represents a query to retrieve a paginated list of expenses.
/// </summary>
/// <remarks>
/// This query is used to fetch a paginated collection of expenses based on the provided filter.
/// </remarks>
/// <summary>
/// Gets the pagination filter used to retrieve the paginated list of expenses.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> representing the pagination options such as page number and page size.
/// </param>
public record GetPaginatedExpensesQuery(PaginationFilter Filter) : IQuery<Pagination<ExpenseDto>>;
