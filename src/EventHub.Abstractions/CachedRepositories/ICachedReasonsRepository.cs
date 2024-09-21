using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.EventAggregate;

namespace EventHub.Abstractions.CachedRepositories;

public interface ICachedReasonsRepository : ICachedRepositoryBase<Reason>
{
}