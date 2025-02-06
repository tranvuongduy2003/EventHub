using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTicketsByUserId;

public record GetPaginatedTicketsByUserIdQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<TicketDto>>;
