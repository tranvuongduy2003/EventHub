using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Notification.GetNotificationsStatistic;

public record GetNotificationsStatisticQuery : IQuery<NotificationStatisticDto>;
