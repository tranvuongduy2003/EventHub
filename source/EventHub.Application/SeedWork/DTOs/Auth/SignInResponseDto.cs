using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Auth;

public class SignInResponseDto
{
    [SwaggerSchema("Access token to login the account")]
    public string AccessToken { get; set; } = string.Empty;

    [SwaggerSchema("Refresh token to get another new access token")]
    public string RefreshToken { get; set; } = string.Empty;
}
