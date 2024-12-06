using System.Security.Claims;
using EventHub.Application.Abstractions;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public FollowCommandHandler(ITokenService tokenService,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AccessToken))
        {
            throw new UnauthorizedException("Unauthorized");
        }
        ClaimsIdentity principal = await _tokenService.GetPrincipalFromToken(request.AccessToken);

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(principal?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "");

        UserAggregateRoot.FollowUser(user?.Id ?? Guid.NewGuid(), request.FollowedUserId);
    }
}
