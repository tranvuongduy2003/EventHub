using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICategoriesRepository : IRepositoryBase<Category>
{
}
