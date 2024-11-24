using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Enums.User;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Auth.SignUp;

public class SignUpCommnadHandler : ICommandHandler<SignUpCommand, SignInResponseDto>
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly SignInManager<Domain.AggregateModels.UserAggregate.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public SignUpCommnadHandler(
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        SignInManager<Domain.AggregateModels.UserAggregate.User> signInManager,
        ITokenService tokenService,
        IHangfireService hangfireService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _hangfireService = hangfireService;
        _emailService = emailService;
    }

    public async Task<SignInResponseDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.UserAggregate.User useByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (useByEmail != null)
        {
            throw new BadRequestException("Email already exists");
        }

        Domain.AggregateModels.UserAggregate.User useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (useByPhoneNumber != null)
        {
            throw new BadRequestException("Phone number already exists");
        }

        var user = new Domain.AggregateModels.UserAggregate.User
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FullName = request.FullName,
            UserName = request.UserName
        };

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            Domain.AggregateModels.UserAggregate.User userToReturn = await _userManager.FindByEmailAsync(request.Email);

            await _userManager.AddToRolesAsync(user, new List<string>
            {
                nameof(EUserRole.CUSTOMER),
                nameof(EUserRole.ORGANIZER)
            });

            await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            string accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            string refreshToken =
                await _userManager
                    .GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

            await _userManager
                .SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

            var signUpResponse = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            if (userToReturn != null && !string.IsNullOrEmpty(userToReturn.Email) && !string.IsNullOrEmpty(userToReturn.FullName))
            {
                _hangfireService.Enqueue(() =>
                    _emailService
                        .SendRegistrationConfirmationEmailAsync(userToReturn.Email, userToReturn.FullName)
                        .Wait());
            }

            return signUpResponse;
        }

        throw new BadRequestException(result);
    }
}