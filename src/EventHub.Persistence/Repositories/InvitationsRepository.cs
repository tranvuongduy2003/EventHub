using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class InvitationsRepository : RepositoryBase<Invitation>, IInvitationsRepository
{
    public InvitationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}