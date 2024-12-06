using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICategoriesRepository : IRepositoryBase<Category>
{
}
