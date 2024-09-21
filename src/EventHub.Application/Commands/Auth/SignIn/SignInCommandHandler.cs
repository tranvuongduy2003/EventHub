using EventHub.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Enums.User;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, SignInResponseDto>
{
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly SignInManager<Domain.AggregateModels.UserAggregate.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public SignInCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        SignInManager<Domain.AggregateModels.UserAggregate.User> signInManager, ITokenService tokenService,
        ILogger<SignInCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<SignInResponseDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: SignInCommandHandler");

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

        _logger.LogInformation("END: SignInCommandHandler");

        var signInResponse = new SignInResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return signInResponse;
    }
}