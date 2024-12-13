using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Event.GetDeletedEventsByUserId;

/// <summary>
/// Represents a query to retrieve a paginated list of events deleted by a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of events that a specified user has deleted, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who deleted the events.
/// </summary>
/// <summary>
/// Gets the pagination filter to apply to the list of deleted events.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetDeletedEventsByUserIdQuery(PaginationFilter Filter) : IQuery<Pagination<EventDto>>;
