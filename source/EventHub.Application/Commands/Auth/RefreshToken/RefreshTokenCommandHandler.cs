using System.Security.Claims;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, SignInResponseDto>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public RefreshTokenCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<SignInResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new InvalidTokenException();
        }

        if (string.IsNullOrEmpty(request.AccessToken))
        {
            throw new UnauthorizedException("Unauthorized");
        }
        ClaimsIdentity principal = await _tokenService.GetPrincipalFromToken(request.AccessToken);

        Domain.AggregateModels.UserAggregate.User user = await _userManager.FindByIdAsync(principal?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "");
        if (user == null)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        bool isValid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenProviders.DEFAULT,
            TokenTypes.REFRESH,
            request.RefreshToken);
        if (!isValid)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        string newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        string newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        var refreshResponse = new SignInResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return refreshResponse;
    }
}
