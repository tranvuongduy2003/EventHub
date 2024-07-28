using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class InvitationsRepository : RepositoryBase<Invitation>, IInvitationsRepository
{
    public InvitationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}