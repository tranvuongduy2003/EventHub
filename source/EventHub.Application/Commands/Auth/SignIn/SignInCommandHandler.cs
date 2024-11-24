using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Enums.User;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, SignInResponseDto>
{
    private readonly SignInManager<Domain.AggregateModels.UserAggregate.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public SignInCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        SignInManager<Domain.AggregateModels.UserAggregate.User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<SignInResponseDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.UserAggregate.User user = _userManager.Users.FirstOrDefault(u =>
            u.Email == request.Identity || u.PhoneNumber == request.Identity);
        if (user == null)
        {
            throw new NotFoundException("Invalid credentials");
        }

        bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        if (user.Status == EUserStatus.INACTIVE)
        {
            throw new UnauthorizedException("Your account was disabled");
        }

        await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        string accessToken = await _tokenService
            .GenerateAccessTokenAsync(user);
        string refreshToken = await _userManager
            .GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

        var signInResponse = new SignInResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return signInResponse;
    }
}
