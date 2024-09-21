using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Application.Queries.Category.GetCategoryById;

/// <summary>
/// Represents a query to retrieve a category's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a category's information based on its unique identifier.
/// </remarks>
public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryDto>;