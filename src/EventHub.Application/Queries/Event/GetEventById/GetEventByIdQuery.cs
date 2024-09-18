using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.Queries.Event.GetEventById;

/// <summary>
/// Represents a query to retrieve an event by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to fetch the details of a specific event using its unique identifier.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event to be retrieved.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record GetEventByIdQuery(Guid EventId) : IQuery<EventDetailDto>;