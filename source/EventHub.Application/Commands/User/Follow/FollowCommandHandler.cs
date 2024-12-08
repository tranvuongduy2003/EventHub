using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public FollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);

        UserAggregateRoot.FollowUser(user?.Id ?? Guid.NewGuid(), request.FollowedUserId);
    }
}
