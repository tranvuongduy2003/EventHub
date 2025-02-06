using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetCreatedEventsPaymentStatistics;

public record GetCreatedEventsPaymentStatisticsQuery(Guid AuthorId) : IQuery<PaymentStatisticsDto>;
