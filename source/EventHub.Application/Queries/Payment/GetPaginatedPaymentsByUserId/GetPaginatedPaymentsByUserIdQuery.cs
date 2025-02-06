using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Payment.GetPaginatedPaymentsByUserId;

public record GetPaginatedPaymentsByUserIdQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<PaymentDto>>;
