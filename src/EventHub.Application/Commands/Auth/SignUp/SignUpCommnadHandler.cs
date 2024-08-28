using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.Enums.User;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.SignUp;

public class SignUpCommnadHandler: ICommandHandler<SignUpCommand, SignInResponseDto>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.AggregateModels.UserAggregate.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly ILogger<SignUpCommnadHandler> _logger;

    public SignUpCommnadHandler(
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, 
        IMapper mapper, SignInManager<Domain.AggregateModels.UserAggregate.User> signInManager, 
        ITokenService tokenService, 
        IHangfireService hangfireService, 
        IEmailService emailService,
        ILogger<SignUpCommnadHandler> logger)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _logger = logger;
    }
    
    public async Task<SignInResponseDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: SignUpCommnadHandler");
        
        var useByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");

        var user = _mapper.Map<Domain.AggregateModels.UserAggregate.User>(request);

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

            var signUpResponse = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            _hangfireService.Enqueue(() =>
                _emailService
                    .SendRegistrationConfirmationEmailAsync(userToReturn.Email, userToReturn.FullName)
                    .Wait());
            
            _logger.LogInformation("END: SignUpCommnadHandler");

            return signUpResponse;
        }

        throw new BadRequestException(result);
    }
}