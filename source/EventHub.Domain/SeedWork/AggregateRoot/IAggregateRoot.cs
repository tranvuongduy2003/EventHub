using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.SeedWork.AggregateRoot;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    void ClearDomainEvents();

    void RaiseDomainEvent(IDomainEvent domainEvent);
}
