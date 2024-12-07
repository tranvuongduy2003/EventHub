using System.Security.Claims;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Auth;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

public class ExternalLoginCallbackCommandHandler : ICommandHandler<ExternalLoginCallbackCommand, SignInResponseDto>
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public ExternalLoginCallbackCommandHandler(
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        IHangfireService hangfireService,
        IEmailService emailService,
        ITokenService tokenService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _tokenService = tokenService;
    }

    public async Task<SignInResponseDto> Handle(ExternalLoginCallbackCommand request,
        CancellationToken cancellationToken)
    {
        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

        string email = info?.Principal.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrEmpty(email))
        {
            Domain.Aggregates.UserAggregate.User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new Domain.Aggregates.UserAggregate.User
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = info!.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    Status = EUserStatus.ACTIVE
                };

                IdentityResult result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new List<string>
                    {
                        nameof(EUserRole.CUSTOMER),
                        nameof(EUserRole.ORGANIZER)
                    });
                }
                else
                {
                    throw new BadRequestException(result);
                }


                _hangfireService.Enqueue(() =>
                    _emailService
                        .SendRegistrationConfirmationEmailAsync(user.Email, user.UserName)
                        .Wait());
            }

            await _signInManager.SignInAsync(user, false);

            string accessToken = await _tokenService
                .GenerateAccessTokenAsync(user);
            string refreshToken = await _userManager
                .GenerateUserTokenAsync(user, info!.LoginProvider, TokenTypes.REFRESH);

            await _userManager
                .SetAuthenticationTokenAsync(user, info.LoginProvider, TokenTypes.REFRESH, refreshToken);

            var signInResponse = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return signInResponse;
        }

        throw new BadRequestException("Failed to sign in");
    }
}
