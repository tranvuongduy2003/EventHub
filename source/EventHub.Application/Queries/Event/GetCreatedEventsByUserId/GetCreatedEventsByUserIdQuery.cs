using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Event.GetCreatedEventsByUserId;

/// <summary>
/// Represents a query to retrieve a paginated list of events created by a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of events that a specified user has created, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who created the events.
/// </summary>
/// <summary>
/// Gets the pagination filter to apply to the list of events.
/// </summary>
/// <param name="Filter">
/// A <see cref="EventPaginationFilter"/> object representing the pagination parameters, including pØage size and page number.
/// </param>
public record GetCreatedEventsByUserIdQuery(EventPaginationFilter Filter) : IQuery<Pagination<EventDto>>;
