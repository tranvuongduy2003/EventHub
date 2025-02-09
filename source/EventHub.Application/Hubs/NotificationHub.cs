using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Hubs;

public class NotificationHub : Hub
{
    private static readonly HashSet<string> _connections = new HashSet<string>(); // Lưu trữ danh sách ConnectionId
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

        // Thêm ConnectionId vào danh sách
        _connections.Add(Context.ConnectionId);

        await base.OnConnectedAsync();

        _logger.LogInformation("END: OnConnectedAsync");
    }

    /// <summary>
    /// Khi người dùng ngắt kết nối khỏi Hub
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("BEGIN: OnDisconnectedAsync - ConnectionId: {ConnectionId}", Context.ConnectionId);

        // Xóa ConnectionId khỏi danh sách
        _connections.Remove(Context.ConnectionId);

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
    public async Task SendNotificationToAll(NotificationDto notification)
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

    /// <summary>
    /// Gửi thông báo đến một nhóm cụ thể
    /// </summary>
    public async Task SendNotificationToGroup(string groupName, SendNotificationDto notification)
    {
        _logger.LogInformation("BEGIN: SendNotificationToGroup - GroupName: {GroupName}", groupName);

        try
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new BadRequestException("Group name is required");
            }

            if (notification == null)
            {
                throw new BadRequestException("Notification data is invalid");
            }

            var notificationEntity = new Notification
            {
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                TargetGroup = groupName,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.Notifications.CreateAsync(notificationEntity);
            await _unitOfWork.CommitAsync();

            NotificationDto notificationDto = _mapper.Map<NotificationDto>(notificationEntity);

            await Clients.Group(groupName).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to group {GroupName}: {Title}", groupName, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to group {GroupName}", groupName);
            throw;
        }

        _logger.LogInformation("END: SendNotificationToGroup");
    }

    /// <summary>
    /// Gửi thông báo đến một người dùng cụ thể
    /// </summary>
    public async Task SendNotificationToUser(string userId, NotificationDto notification)
    {
        _logger.LogInformation("BEGIN: SendNotificationToUser - UserId: {UserId}", userId);

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
                TargetUserId = Guid.Parse(userId),
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.Notifications.CreateAsync(notificationEntity);
            await _unitOfWork.CommitAsync();

            notificationEntity.TargetUser = user;
            NotificationDto notificationDto = _mapper.Map<NotificationDto>(notificationEntity);

            await Clients.User(userId).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
            throw;
        }

        _logger.LogInformation("END: SendNotificationToUser");
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
