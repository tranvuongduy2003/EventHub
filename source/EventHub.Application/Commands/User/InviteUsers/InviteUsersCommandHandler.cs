using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.InviteUsers;

public class InviteUsersCommandHandler : ICommandHandler<InviteUsersCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public InviteUsersCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task Handle(InviteUsersCommand request, CancellationToken cancellationToken)
    {
        var inviterId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)
        ?.Value!);

        foreach (Guid userId in request.UserIds.Distinct())
        {
            bool isInvitationExisted = await _unitOfWork.Invitations.ExistAsync(x => x.InviterId == inviterId && x.InvitedId == userId && x.EventId == request.EventId);
            if (isInvitationExisted)
            {
                continue;
            }

            var invitation = new Invitation
            {
                InvitedId = userId,
                InviterId = inviterId,
                EventId = request.EventId,
            };
            await _unitOfWork.Invitations.CreateAsync(invitation);
            await _unitOfWork.CommitAsync();

            Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
            Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(inviterId.ToString());
            var notification = new SendNotificationDto
            {
                Title = "You have been invited to an event",
                Message = $"You have been invited to event {@event.Name} by {user!.FullName}",
                Type = ENotificationType.INVITING,
            };
            await _notificationService.SendNotification(userId.ToString(), notification);
        }
    }
}
