using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Payment.GetPaginatedPayments;

public record GetPaginatedPaymentsQuery(PaginationFilter Filter) : IQuery<Pagination<PaymentDto>>;
