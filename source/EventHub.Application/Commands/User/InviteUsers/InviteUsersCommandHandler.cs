using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.InviteUsers;

public class InviteUsersCommandHandler : ICommandHandler<InviteUsersCommand, List<Guid>>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public InviteUsersCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IUnitOfWork unitOfWork)
    {
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
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
        }

        return invitationIds;
    }
}
