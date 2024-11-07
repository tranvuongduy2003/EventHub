using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.CategoryAggregate;

namespace EventHub.Abstractions.CachedRepositories;

public interface ICachedCategoriesRepository : ICachedRepositoryBase<Category>
{
}