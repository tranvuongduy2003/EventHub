using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class FunctionsRepository : RepositoryBase<Function>, IFunctionsRepository
{
    public FunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}