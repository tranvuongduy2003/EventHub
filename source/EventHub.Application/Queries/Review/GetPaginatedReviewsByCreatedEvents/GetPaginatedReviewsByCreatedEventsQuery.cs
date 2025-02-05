using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByCreatedEvents;

/// <summary>
/// Represents a query to retrieve a paginated list of reviews.
/// </summary>
/// <remarks>
/// This query is used to fetch a paginated collection of reviews based on the provided filter.
/// </remarks>
/// <summary>
/// Gets the pagination filter used to retrieve the paginated list of reviews.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> representing the pagination options such as page number and page size.
/// </param>
public record GetPaginatedReviewsByCreatedEventsQuery(PaginationFilter Filter) : IQuery<Pagination<ReviewDto>>;
