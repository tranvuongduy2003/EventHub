using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class LabelInUsersRepository : RepositoryBase<LabelInUser>, ILabelInUsersRepository
{
    public LabelInUsersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}