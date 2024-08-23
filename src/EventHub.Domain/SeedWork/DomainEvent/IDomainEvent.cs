using MediatR;

namespace EventHub.Domain.SeedWork.DomainEvent;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }
}