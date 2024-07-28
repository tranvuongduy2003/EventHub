using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
{
    public TicketsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}