namespace EventHub.Domain.SeedWork.DomainEvent;

/// <summary>
/// Represents the base class for a domain event in the system.
/// </summary>
/// <remarks>
/// This abstract record provides a common implementation for domain events, including a unique
/// identifier. It implements the <see cref="IDomainEvent"/> interface, ensuring that all domain
/// events derived from this base class will have a unique ID and adhere to the domain event contract.
/// </remarks>
/// <param name="Id">
/// The unique identifier of the domain event.
/// </param>
public abstract record DomainEvent(Guid Id) : IDomainEvent;
