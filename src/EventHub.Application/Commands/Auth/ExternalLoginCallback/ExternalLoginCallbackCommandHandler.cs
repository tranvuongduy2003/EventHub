using System.Security.Claims;
using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Enums.User;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

public class ExternalLoginCallbackCommandHandler : ICommandHandler<ExternalLoginCallbackCommand, SignInResponseDto>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<ExternalLoginCallbackCommandHandler> _logger;

    public ExternalLoginCallbackCommandHandler(
        SignInManager<User> signInManager, 
        UserManager<User> userManager,
        IHangfireService hangfireService, 
        IEmailService emailService, 
        ITokenService tokenService,
        ILogger<ExternalLoginCallbackCommandHandler> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public async Task<SignInResponseDto> Handle(ExternalLoginCallbackCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ExternalLoginCallbackCommandHandler");
        
        var info = await _signInManager.GetExternalLoginInfoAsync();

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrEmpty(email))
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    Status = EUserStatus.ACTIVE
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                    await _userManager.AddToRolesAsync(user, new List<string>
                    {
                        nameof(EUserRole.CUSTOMER),
                        nameof(EUserRole.ORGANIZER)
                    });
                else
                    throw new BadRequestException(result);


                _hangfireService.Enqueue(() =>
                    _emailService
                        .SendRegistrationConfirmationEmailAsync(user.Email, user.UserName)
                        .Wait());
            }

            await _signInManager.SignInAsync(user, false);

            var accessToken = await _tokenService
                .GenerateAccessTokenAsync(user);
            var refreshToken = await _userManager
                .GenerateUserTokenAsync(user, info.LoginProvider, TokenTypes.REFRESH);

            await _userManager
                .SetAuthenticationTokenAsync(user, info.LoginProvider, TokenTypes.REFRESH, refreshToken);

            var signInResponse = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            
            _logger.LogInformation("END: ExternalLoginCallbackCommandHandler");

            return signInResponse;
        }

        throw new BadRequestException("Failed to sign in");
    }
}