using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Payment.GetPaginatedPaymentsByEventId;

public record GetPaginatedPaymentsByEventIdQuery(Guid EventId, PaginationFilter Filter) : IQuery<Pagination<PaymentDto>>;
