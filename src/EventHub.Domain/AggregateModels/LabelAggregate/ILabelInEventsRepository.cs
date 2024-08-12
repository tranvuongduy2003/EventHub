using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.LabelAggregate;

public interface ILabelInEventsRepository : IRepositoryBase<LabelInEvent>
{
}