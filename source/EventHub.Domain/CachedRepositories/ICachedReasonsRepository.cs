using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedReasonsRepository : ICachedRepositoryBase<Reason>
{
}