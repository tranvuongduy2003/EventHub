using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetUserPaymentStatistics;

public record GetUserPaymentStatisticsQuery(Guid UserId) : IQuery<PaymentStatisticsDto>;
