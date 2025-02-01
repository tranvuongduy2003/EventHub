using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Event.GetFavouriteEventsByUserId;

/// <summary>
/// Represents a query to retrieve a paginated list of events marked as favorites by a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of events that a specified user has marked as favorites, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who marked the events as favorites.
/// </summary>
/// <summary>
/// Gets the pagination filter to apply to the list of favorite events.
/// </summary>
/// <param name="Filter">
/// A <see cref="EventPaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetFavouriteEventsByUserIdQuery(EventPaginationFilter Filter) : IQuery<Pagination<EventDto>>;
