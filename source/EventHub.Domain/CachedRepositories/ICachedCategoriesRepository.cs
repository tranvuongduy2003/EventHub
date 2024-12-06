using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedCategoriesRepository : ICachedRepositoryBase<Category>
{
}