using EventHub.Application.Commands.Auth.RefreshToken;
using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, SignInResponseDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(UserManager<User> userManager, ITokenService tokenService, ILogger<RefreshTokenCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public async Task<SignInResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: RefreshTokenCommandHandler");
        
        if (string.IsNullOrEmpty(request.RefreshToken))
            throw new InvalidTokenException();

        if (string.IsNullOrEmpty(request.AccessToken)) throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(request.AccessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var isValid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenProviders.DEFAULT,
            TokenTypes.REFRESH,
            request.RefreshToken);
        if (!isValid) throw new UnauthorizedException("Unauthorized");

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        _logger.LogInformation("END: RefreshTokenCommandHandler");
        
        var refreshResponse = new SignInResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return refreshResponse;
    }
}