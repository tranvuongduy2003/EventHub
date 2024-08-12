using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class LabelsRepository : RepositoryBase<Label>, ILabelsRepository
{
    public LabelsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}