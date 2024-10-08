using EventHub.Abstractions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand>
{
    private readonly ILogger<UnfollowCommandHandler> _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public UnfollowCommandHandler(ILogger<UnfollowCommandHandler> logger, ITokenService tokenService,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UnfollowCommandHandler");

        if (string.IsNullOrEmpty(request.AccessToken))
            throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(request.AccessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

        await UserAggregateRoot.UnfollowUser(user?.Id ?? default, request.FollowedUserId);

        _logger.LogInformation("END: UnfollowCommandHandler");
    }
}