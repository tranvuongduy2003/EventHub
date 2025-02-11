using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Notification.GetPaginatedNotifications;

public record GetPaginatedNotificationsQuery(NotificationPaginationFilter Filter) : IQuery<Pagination<NotificationDto, NotificationMetadata>>;
