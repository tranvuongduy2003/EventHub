using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface INotificationsRepository : IRepositoryBase<Notification>
{
}
