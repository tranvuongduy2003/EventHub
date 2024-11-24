using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Abstractions.SeedWork.Repository;

/// <summary>
/// Defines the contract for a repository that manages entities of type <typeparamref name="T"/> with caching capabilities.
/// </summary>
/// <typeparam name="T">The type of the entity managed by this repository. It must derive from <see cref="EntityBase"/>.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRepositoryBase{T}"/> and is intended for repositories that include caching functionality.
/// It provides the same methods as <see cref="IRepositoryBase{T}"/> but may include additional methods specific to caching operations.
/// </remarks>
public interface ICachedRepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
}
