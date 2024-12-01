using System.Security.Claims;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public UnfollowCommandHandler(ITokenService tokenService,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AccessToken))
        {
            throw new UnauthorizedException("Unauthorized");
        }
        ClaimsIdentity principal = await _tokenService.GetPrincipalFromToken(request.AccessToken);

        Domain.AggregateModels.UserAggregate.User user = await _userManager.FindByIdAsync(principal?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "");

        UserAggregateRoot.UnfollowUser(user?.Id ?? Guid.NewGuid(), request.FollowedUserId);
    }
}
