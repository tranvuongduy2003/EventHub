using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class LabelInEventsRepository : RepositoryBase<LabelInEvent>, ILabelInEventsRepository
{
    public LabelInEventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}