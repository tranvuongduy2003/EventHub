using AutoMapper;
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

namespace EventHub.Application.Hubs;

public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public NotificationHub(ILogger<NotificationHub> logger, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Khi người dùng kết nối với Hub
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("BEGIN: OnConnectedAsync - ConnectionId: {ConnectionId}", Context.ConnectionId);

        await base.OnConnectedAsync();

        _logger.LogInformation("END: OnConnectedAsync");
    }

    /// <summary>
    /// Khi người dùng ngắt kết nối khỏi Hub
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("BEGIN: OnDisconnectedAsync - ConnectionId: {ConnectionId}", Context.ConnectionId);

        if (exception != null)
        {
            _logger.LogError(exception, "Error during disconnection for ConnectionId: {ConnectionId}", Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);

        _logger.LogInformation("END: OnDisconnectedAsync");
    }

    /// <summary>
    /// Gửi thông báo đến tất cả người dùng
    /// </summary>
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
            await Clients.All.SendAsync("ReceiveNotification", notification);

            _logger.LogInformation("Notification sent to all users: {Title}", notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to all users");
            throw;
        }

        _logger.LogInformation("END: SendNotificationToAll");
    }

    public async Task Connect(string userId)
    {
        _logger.LogInformation("BEGIN: Connect - ConnectionId: {ConnectionId} {UserId}", Context.ConnectionId, userId);

        await Groups.AddToGroupAsync(Context.ConnectionId, userId);

        _logger.LogInformation("END: Connect");
    }

    /// <summary>
    /// Gửi thông báo đến một người dùng cụ thể
    /// </summary>
    public async Task SendNotification(string userId, SendNotificationDto notification)
    {
        _logger.LogInformation("BEGIN: SendNotification - UserId: {UserId}", userId);

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

            await Clients.Group(userId).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
            throw;
        }

        _logger.LogInformation("END: SendNotification");
    }

    /// <summary>
    /// Tham gia vào một nhóm
    /// </summary>
    public async Task JoinGroup(string groupName)
    {
        _logger.LogInformation("BEGIN: JoinGroup - GroupName: {GroupName}, ConnectionId: {ConnectionId}", groupName, Context.ConnectionId);

        try
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new BadRequestException("Group name is required");
            }

            // Thêm ConnectionId vào nhóm
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            _logger.LogInformation("User joined group {GroupName}", groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error joining group {GroupName}", groupName);
            throw;
        }

        _logger.LogInformation("END: JoinGroup");
    }

    /// <summary>
    /// Rời khỏi một nhóm
    /// </summary>
    public async Task LeaveGroup(string groupName)
    {
        _logger.LogInformation("BEGIN: LeaveGroup - GroupName: {GroupName}, ConnectionId: {ConnectionId}", groupName, Context.ConnectionId);

        try
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new BadRequestException("Group name is required");
            }

            // Xóa ConnectionId khỏi nhóm
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            _logger.LogInformation("User left group {GroupName}", groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error leaving group {GroupName}", groupName);
            throw;
        }

        _logger.LogInformation("END: LeaveGroup");
    }
}
