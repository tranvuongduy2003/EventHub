using System.Security.Claims;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public UnfollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Claims
            .FirstOrDefault(x => x.Equals(JwtRegisteredClaimNames.Jti))
            ?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);

        UserAggregateRoot.UnfollowUser(user?.Id ?? Guid.NewGuid(), request.FollowedUserId);
    }
}
