using EventHub.Application.SeedWork.DTOs.Statistic;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetEventReviewsByCustomer;

public record GetEventReviewsByCustomerQuery(Guid EventId) : IQuery<EventReviewsByCustomerDto>;
