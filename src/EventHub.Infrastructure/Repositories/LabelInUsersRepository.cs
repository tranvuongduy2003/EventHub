using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class LabelInUsersRepository : RepositoryBase<LabelInUser>, ILabelInUsersRepository
{
    public LabelInUsersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}