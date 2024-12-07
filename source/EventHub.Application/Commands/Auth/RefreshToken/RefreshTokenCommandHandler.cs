using System.Security.Claims;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Auth;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, SignInResponseDto>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public RefreshTokenCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

        string userId = _signInManager.Context.User.Claims.FirstOrDefault(x => x.Equals(JwtRegisteredClaimNames.Jti))
            ?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);
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
