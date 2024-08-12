using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class InvitationsRepository : RepositoryBase<Invitation>, IInvitationsRepository
{
    public InvitationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}