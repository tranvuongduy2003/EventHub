using MediatR;

namespace EventHub.Domain.SeedWork.DomainEvent;

/// <summary>
/// Represents a domain event in a system that uses event notification patterns.
/// </summary>
/// <remarks>
/// This interface extends <see cref="INotification"/> and is intended to define domain events
/// that can be used to notify different parts of the system about significant changes or occurrences.
/// Domain events typically encapsulate information about a state change or an important event in the domain.
/// </remarks>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the domain event.
    /// This ID is used to distinguish this event from other events and can be useful for tracking or 
    /// correlating events across the system.
    /// </value>
    Guid Id { get; init; }
}