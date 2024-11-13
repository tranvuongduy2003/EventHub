using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByUserId;

/// <summary>
/// Represents a query to retrieve a paginated list of reviews created by a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch a paginated collection of reviews based on the user's unique identifier and the provided pagination filter.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user whose reviews are being retrieved.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the pagination filter used to retrieve the paginated list of reviews.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> representing the pagination options such as page number and page size.
/// </param>
public record GetPaginatedReviewsByUserIdQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<ReviewDto>>;