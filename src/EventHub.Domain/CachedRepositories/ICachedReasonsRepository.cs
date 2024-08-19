using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedReasonsRepository : ICachedRepositoryBase<Reason>
{
}