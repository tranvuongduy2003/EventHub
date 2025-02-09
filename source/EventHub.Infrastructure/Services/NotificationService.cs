﻿using AutoMapper;
using EventHub.Application.Hubs;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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

            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to group {GroupName}: {Title}", groupName, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to group {GroupName}", groupName);
            throw;
        }

        _logger.LogInformation("END: SendNotificationToGroup");
    }

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

            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notificationDto);

            _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
            throw;
        }

        _logger.LogInformation("END: SendNotificationToUser");
    }
}
