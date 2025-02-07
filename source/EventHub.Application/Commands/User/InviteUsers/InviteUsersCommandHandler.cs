using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.InviteUsers;

public class InviteUsersCommandHandler : ICommandHandler<InviteUsersCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public InviteUsersCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
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
        }
    }
}
