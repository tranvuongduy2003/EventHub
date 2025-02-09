using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

public record CalculatePositivePercentageOfReviewDomainEvent(Guid Id, Guid ReviewId) : DomainEvent(Id);
