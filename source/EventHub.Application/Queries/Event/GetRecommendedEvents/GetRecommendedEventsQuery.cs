using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetRecommendedEvents;

public record GetRecommendedEventsQuery : IQuery<List<EventDto>>;
