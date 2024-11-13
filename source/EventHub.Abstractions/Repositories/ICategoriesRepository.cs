using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.CategoryAggregate;

namespace EventHub.Abstractions.Repositories;

public interface ICategoriesRepository : IRepositoryBase<Category>
{
}