using EventHub.Domain.Aggregates.LabelAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class LabelInUsersRepository : RepositoryBase<LabelInUser>, ILabelInUsersRepository
{
    public LabelInUsersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}