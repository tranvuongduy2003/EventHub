using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetEventPaymentStatistics;

public record GetEventPaymentStatisticsQuery(Guid EventId) : IQuery<PaymentStatisticsDto>;
