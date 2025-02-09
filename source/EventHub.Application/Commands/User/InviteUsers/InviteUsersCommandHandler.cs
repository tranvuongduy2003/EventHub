using EventHub.Application.Hubs;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.InviteUsers;

public class InviteUsersCommandHandler : ICommandHandler<InviteUsersCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub> _hubContext;

    public InviteUsersCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
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
            var notification = new NotificationDto
            {
                Title = "You have been invited to an event",
                Message = $"You have been invited to event {@event.Name} by {user!.FullName}",
                Type = ENotificationType.INVITING,
            };
            await _hubContext.Clients.User(userId.ToString()).SendAsync("SendNotificationToUser", userId, notification, cancellationToken);
        }
    }
}
