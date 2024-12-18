using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.SeedWork.AggregateRoot;

public abstract class AggregateRoot : EntityBase, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
