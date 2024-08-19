using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.SeedWork.Repository;

public interface ICachedRepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
}