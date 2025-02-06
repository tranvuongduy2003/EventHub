using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTicketsByEventId;

/// <summary>
/// Represents a query to retrieve a paginated list of expenses for a specific event.
/// </summary>
/// <remarks>
/// This query is used to fetch a paginated collection of expenses based on the event's unique identifier and the provided pagination filter.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event for which the expenses are being retrieved.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the pagination filter used to retrieve the paginated list of expenses.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> representing the pagination options such as page number and page size.
/// </param>
public record GetPaginatedTicketsByEventIdQuery(Guid EventId, PaginationFilter Filter) : IQuery<Pagination<TicketDto>>;
