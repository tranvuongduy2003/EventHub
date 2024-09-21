using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.LabelAggregate;

namespace EventHub.Abstractions.Repositories;

public interface ILabelInUsersRepository : IRepositoryBase<LabelInUser>
{
}