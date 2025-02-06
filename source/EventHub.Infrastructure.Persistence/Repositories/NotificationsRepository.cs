using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class NotificationsRepository : RepositoryBase<Notification>, INotificationsRepository
{
    public NotificationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
