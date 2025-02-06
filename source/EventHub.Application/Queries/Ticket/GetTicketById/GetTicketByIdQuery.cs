using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Ticket.GetTicketById;

/// <summary>
/// Represents a query to retrieve a ticket's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a ticket's information based on its unique identifier.
/// </remarks>
public record GetTicketByIdQuery(Guid TicketId) : IQuery<TicketDto>;
