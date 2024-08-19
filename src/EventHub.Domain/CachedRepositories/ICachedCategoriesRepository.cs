using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedCategoriesRepository : ICachedRepositoryBase<Category>
{
}