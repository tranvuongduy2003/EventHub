using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class SubExpensesRepository : RepositoryBase<SubExpense>, ISubExpensesRepository
{
    public SubExpensesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
