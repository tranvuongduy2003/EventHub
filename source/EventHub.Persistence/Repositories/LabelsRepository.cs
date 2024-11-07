using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class LabelsRepository : RepositoryBase<Label>, ILabelsRepository
{
    public LabelsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}