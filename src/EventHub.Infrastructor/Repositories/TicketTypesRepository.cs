using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class TicketTypesRepository : RepositoryBase<TicketType>, ITicketTypesRepository
{
    public TicketTypesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}