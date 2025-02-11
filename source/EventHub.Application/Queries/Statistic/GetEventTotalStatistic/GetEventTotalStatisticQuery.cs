using EventHub.Application.SeedWork.DTOs.Statistic;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetEventTotalStatistic;

public record GetEventTotalStatisticQuery(Guid EventId) : IQuery<EventTotalStatisticDto>;
