using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class InvitationsRepository : RepositoryBase<Invitation>, IInvitationsRepository
{
    public InvitationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}