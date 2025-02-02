using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetCreatedEventsStatistics;

public record GetCreatedEventsStatisticsQuery() : IQuery<CreatedEventsStatisticsDto>;
