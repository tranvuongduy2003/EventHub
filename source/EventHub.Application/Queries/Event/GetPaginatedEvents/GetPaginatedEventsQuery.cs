using EventHub.Application.DTOs.Event;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Event.GetPaginatedEvents;

/// <summary>
/// Represents a query to retrieve a paginated list of events.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of events, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the pagination filter to apply to the events list.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetPaginatedEventsQuery(PaginationFilter Filter) : IQuery<Pagination<EventDto>>;
