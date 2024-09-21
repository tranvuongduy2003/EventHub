using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;

/// <summary>
/// Represents a query to retrieve a paginated list of reviews for a specific event.
/// </summary>
/// <remarks>
/// This query is used to fetch a paginated collection of reviews based on the event's unique identifier and the provided pagination filter.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event for which the reviews are being retrieved.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the pagination filter used to retrieve the paginated list of reviews.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> representing the pagination options such as page number and page size.
/// </param>
public record GetPaginatedReviewsByEventIdQuery(Guid EventId, PaginationFilter Filter) : IQuery<Pagination<ReviewDto>>;