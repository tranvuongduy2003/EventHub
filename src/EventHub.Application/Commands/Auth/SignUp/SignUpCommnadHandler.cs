using AutoMapper;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Shared.Enums.User;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Models.Auth;
using EventHub.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Auth.SignUp;

public class SignUpCommnadHandler: IRequestHandler<SignUpCommand, SignInResponseModel>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;

    public SignUpCommnadHandler(
        UserManager<User> userManager, 
        IMapper mapper, SignInManager<User> signInManager, 
        ITokenService tokenService, 
        IHangfireService hangfireService, 
        IEmailService emailService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _hangfireService = hangfireService;
        _emailService = emailService;
    }
    
    public async Task<SignInResponseModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var useByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");

        var user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var userToReturn = await _userManager.FindByEmailAsync(request.Email);

            await _userManager.AddToRolesAsync(user, new List<string>
            {
                nameof(EUserRole.CUSTOMER),
                nameof(EUserRole.ORGANIZER)
            });

            await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken =
                await _userManager
                    .GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

            await _userManager
                .SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

            var signUpResponse = new SignInResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            _hangfireService.Enqueue(() =>
                _emailService
                    .SendRegistrationConfirmationEmailAsync(userToReturn.Email, userToReturn.FullName)
                    .Wait());

            return signUpResponse;
        }

        throw new BadRequestException(result);
    }
}