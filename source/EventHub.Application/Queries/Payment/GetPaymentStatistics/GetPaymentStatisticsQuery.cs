using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetPaymentStatistics;

public record GetPaymentStatisticsQuery() : IQuery<PaymentStatisticsDto>;
