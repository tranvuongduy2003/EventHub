using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class LabelInEventsRepository : RepositoryBase<LabelInEvent>, ILabelInEventsRepository
{
    public LabelInEventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}