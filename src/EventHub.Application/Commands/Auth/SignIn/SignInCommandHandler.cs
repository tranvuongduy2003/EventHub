using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Shared.Enums.User;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Models.Auth;
using EventHub.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponseModel>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public SignInCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    public async Task<SignInResponseModel> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = _userManager.Users.FirstOrDefault(u =>
            u.Email == request.Identity || u.PhoneNumber == request.Identity);
        if (user == null)
            throw new NotFoundException("Invalid credentials");

        var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (isValid == false) throw new UnauthorizedException("Invalid credentials");

        if (user.Status == EUserStatus.INACTIVE)
            throw new UnauthorizedException("Your account was disabled");

        await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        var accessToken = await _tokenService
            .GenerateAccessTokenAsync(user);
        var refreshToken = await _userManager
            .GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

        var signInResponse = new SignInResponseModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return signInResponse;
    }
}