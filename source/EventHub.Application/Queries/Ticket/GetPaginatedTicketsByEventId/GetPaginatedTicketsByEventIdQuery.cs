using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTicketsByEventId;

public record GetPaginatedTicketsByEventIdQuery(Guid EventId, PaginationFilter Filter) : IQuery<Pagination<TicketDto>>;
