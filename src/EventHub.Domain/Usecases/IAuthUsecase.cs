using EventHub.Domain.DTOs.Auth;
using EventHub.Domain.DTOs.User;
using EventHub.Domain.Models.Auth;
using EventHub.Domain.Models.User;

namespace EventHub.Domain.Usecases;

public interface IAuthUsecase
{
    Task<SignInResponseModel> SignUpAsync(CreateUserDto dto);

    Task<bool> ValidateUserAsync(ValidateUserDto dto);

    Task<SignInResponseModel> SignInAsync(SignInDto dto);

    Task<bool> SignOutAsync();

    Task<ExternalLoginModel> ExternalLoginAsync(string provider, string returnUrl);

    Task<SignInResponseModel> ExternalLoginCallbackAsync(string returnUrl);

    Task<SignInResponseModel> RefreshTokenAsync(RefreshTokenDto dto, string? accessToken);

    Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);

    Task<bool> ResetPasswordAsync(ResetUserPasswordDto dto);

    Task<UserModel> GetUserProfileAsync(string accessToken);
}