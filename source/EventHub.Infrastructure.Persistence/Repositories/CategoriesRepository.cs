using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
{
    public CategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}