using EventHub.Abstractions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public FollowCommandHandler(ITokenService tokenService,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AccessToken))
            throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(request.AccessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

        await UserAggregateRoot.FollowUser(user?.Id ?? default, request.FollowedUserId);
    }
}