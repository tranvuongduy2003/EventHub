using AutoMapper;
using EventHub.Application.Hubs;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(ILogger<NotificationService> logger, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IHubContext<NotificationHub> hubContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    public async Task SendNotificationToAll(SendNotificationDto notification)
    {
        _logger.LogInformation("BEGIN: SendNotificationToAll");

        try
        {
            // Kiểm tra dữ liệu đầu vào
            if (notification == null)
            {
                throw new BadRequestException("Notification data is invalid");
            }

            // Lưu thông báo vào cơ sở dữ liệu
            var notificationEntity = new Notification
            {
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.Notifications.CreateAsync(notificationEntity);
            await _unitOfWork.CommitAsync();

            // Gửi thông báo đến tất cả client
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

            _logger.LogInformation("Notification sent to all users: {Title}", notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to all users");
            throw;
        }

        _logger.LogInformation("END: SendNotificationToAll");
    }

    public async Task SendNotification(string userId, SendNotificationDto notification)
    {
        _logger.LogInformation("BEGIN: SendNotificationToGroup - GroupName: {GroupName}", userId);

        try
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User does not exist!");
            }

            if (notification == null)
            {
                throw new BadRequestException("Notification data is invalid!");
            }

            // Lưu thông báo vào cơ sở dữ liệu
            var notificationEntity = new Notification
            {
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                TargetGroup = userId,
                TargetUserId = Guid.Parse(userId),
                Timestamp = DateTime.UtcNow,
                InvitationId = notification.InvitationId,
                PaymentId = notification.PaymentId,
                UserFollowerId = notification.UserFollowerId
            };

            await _unitOfWork.Notifications.CreateAsync(notificationEntity);
            await _unitOfWork.CommitAsync();

            if (notification.Type == Domain.Shared.Enums.Notification.ENotificationType.FOLLOWING && notification.UserFollowerId is not null)
            {
                UserFollower userFollower = await _unitOfWork.UserFollowers
                    .FindByCondition(x => x.Id == notification.UserFollowerId)
                    .Include(x => x.Follower)
                    .FirstOrDefaultAsync();
                notificationEntity.UserFollower = userFollower;
            }
            else if (notification.Type == Domain.Shared.Enums.Notification.ENotificationType.INVITING && notification.InvitationId is not null)
            {
                Invitation invitation = await _unitOfWork.Invitations
                    .FindByCondition(x => x.Id == notification.InvitationId)
                    .Include(x => x.Invited)
                    .Include(x => x.Inviter)
                    .Include(x => x.Event)
                    .FirstOrDefaultAsync();
                notificationEntity.Invitation = invitation;
            }
            else if (notification.Type == Domain.Shared.Enums.Notification.ENotificationType.ORDERING && notification.PaymentId is not null)
            {
                Payment payment = await _unitOfWork.Payments
                    .FindByCondition(x => x.Id == notification.PaymentId)
                    .Include(x => x.Author)
                    .Include(x => x.Event)
                    .FirstOrDefaultAsync();
                notificationEntity.Payment = payment;
            }


            notificationEntity.TargetUser = user;
            NotificationDto notificationDto = _mapper.Map<NotificationDto>(notificationEntity);

            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to group {GroupName}: {Title}", userId, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to group {GroupName}", userId);
            throw;
        }

        _logger.LogInformation("END: SendNotificationToGroup");
    }
}
