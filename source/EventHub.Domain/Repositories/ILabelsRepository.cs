using EventHub.Domain.Aggregates.LabelAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ILabelsRepository : IRepositoryBase<Label>
{
}