using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface ICategoriesRepository : IRepositoryBase<Category>
{
}