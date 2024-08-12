using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class LabelInEventsRepository : RepositoryBase<LabelInEvent>, ILabelInEventsRepository
{
    public LabelInEventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}