using EventHub.Application.DTOs.Category;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

/// <summary>
/// Represents a query to retrieve a paginated list of categories based on a pagination filter.
/// </summary>
/// <remarks>
/// This query is used to request a list of categories with pagination, using the specified filter parameters to control the paging behavior.
/// </remarks>
public record GetPaginatedCategoriesQuery(PaginationFilter Filter) : IQuery<Pagination<CategoryDto>>;
