using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class ReasonsRepository : RepositoryBase<Reason>, IReasonsRepository
{
    public ReasonsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}