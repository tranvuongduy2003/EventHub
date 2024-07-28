using System.Security.Claims;
using AutoMapper;
using EventHub.Domain.Common.Entities;
using EventHub.Domain.DTOs.Auth;
using EventHub.Domain.DTOs.User;
using EventHub.Domain.Enums.User;
using EventHub.Domain.Exceptions;
using EventHub.Domain.Interfaces;
using EventHub.Domain.Models.Auth;
using EventHub.Domain.Models.User;
using EventHub.Domain.Usecases;
using EventHub.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using IHangfireService = EventHub.Domain.Hangfire.IHangfireService;

namespace EventHub.Usecase.Usecases;

public class AuthUsecase : IAuthUsecase
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public AuthUsecase(UserManager<User> userManager,
        SignInManager<User> signInManager, ITokenService tokenService, IEmailService emailService,
        IHangfireService hangfireService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _hangfireService = hangfireService;
        _mapper = mapper;
    }

    public async Task<SignInResponseModel> SignUpAsync(CreateUserDto dto)
    {
        var useByEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");

        var user = _mapper.Map<User>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
        {
            var userToReturn = await _userManager.FindByEmailAsync(dto.Email);

            await _userManager.AddToRolesAsync(user, new List<string>
            {
                nameof(EUserRole.CUSTOMER),
                nameof(EUserRole.ORGANIZER)
            });

            await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

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

    public async Task<bool> ValidateUserAsync(ValidateUserDto dto)
    {
        var useByEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");

        return true;
    }

    public async Task<SignInResponseModel> SignInAsync(SignInDto dto)
    {
        var user = _userManager.Users.FirstOrDefault(u =>
            u.Email == dto.Identity || u.PhoneNumber == dto.Identity);
        if (user == null)
            throw new NotFoundException("Invalid credentials");

        var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (isValid == false) throw new UnauthorizedException("Invalid credentials");

        if (user.Status == EUserStatus.INACTIVE)
            throw new UnauthorizedException("Your account was disabled");

        await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

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

    public async Task<bool> SignOutAsync()
    {
        await _signInManager.SignOutAsync();

        return true;
    }

    public async Task<ExternalLoginModel> ExternalLoginAsync(string provider, string returnUrl)
    {
        var redirectUrl =
            $"https://eventhubsolutionbackendserverplan.azurewebsites.net/api/auth/external-auth-callback?returnUrl={returnUrl}";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.AllowRefresh = true;

        var externalLoginResponse = new ExternalLoginModel
        {
            Properties = properties,
            Provider = provider
        };

        return externalLoginResponse;
    }

    public async Task<SignInResponseModel> ExternalLoginCallbackAsync(string returnUrl)
    {
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

            var signInResponse = new SignInResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return signInResponse;
        }

        throw new BadRequestException("Failed to sign in");
    }

    public async Task<SignInResponseModel> RefreshTokenAsync(RefreshTokenDto dto, string? accessToken)
    {
        if (string.IsNullOrEmpty(dto.RefreshToken))
            throw new InvalidTokenException();

        if (string.IsNullOrEmpty(accessToken)) throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var isValid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenProviders.DEFAULT,
            TokenTypes.REFRESH,
            dto.RefreshToken);
        if (!isValid) throw new UnauthorizedException("Unauthorized");

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        var refreshResponse = new SignInResponseModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return refreshResponse;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPasswordUrl = $"https://localhost:5173/reset-password?token={token}&email={dto.Email}";

        _hangfireService.Enqueue(() =>
            _emailService
                .SendResetPasswordEmailAsync(dto.Email, resetPasswordUrl)
                .Wait());

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetUserPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded) throw new BadRequestException(result);

        return true;
    }

    public async Task<UserModel> GetUserProfileAsync(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken)) throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var roles = await _userManager.GetRolesAsync(user);
        var userModel = _mapper.Map<UserModel>(user);
        userModel.Roles = roles.ToList();

        return userModel;
    }
}