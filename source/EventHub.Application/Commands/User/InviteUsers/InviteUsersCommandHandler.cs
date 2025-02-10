using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.InviteUsers;

public class InviteUsersCommandHandler : ICommandHandler<InviteUsersCommand, List<Guid>>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public InviteUsersCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<List<Guid>> Handle(InviteUsersCommand request, CancellationToken cancellationToken)
    {
        var inviterId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)
        ?.Value!);

        var invitationIds = new List<Guid>();
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

            invitationIds.Add(invitation.Id);

            var notification = new SendNotificationDto
            {
                Type = ENotificationType.INVITING,
                InvitationId = invitation.Id,
            };
            await _notificationService.SendNotification(userId.ToString(), notification);
        }

        return invitationIds;
    }
}
