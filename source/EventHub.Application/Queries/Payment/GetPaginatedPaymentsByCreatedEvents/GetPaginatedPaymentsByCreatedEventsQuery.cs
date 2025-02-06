using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Payment.GetPaginatedPaymentsByCreatedEvents;

public record GetPaginatedPaymentsByCreatedEventsQuery(Guid AuthorId, PaginationFilter Filter) : IQuery<Pagination<PaymentDto>>;
