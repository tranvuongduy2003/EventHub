using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class ExpensesRepository : RepositoryBase<Expense>, IExpensesRepository
{
    public ExpensesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
