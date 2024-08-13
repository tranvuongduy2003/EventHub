using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class LabelInUsersRepository : RepositoryBase<LabelInUser>, ILabelInUsersRepository
{
    public LabelInUsersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}