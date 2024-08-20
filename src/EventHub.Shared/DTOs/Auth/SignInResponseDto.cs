using System.ComponentModel;

namespace EventHub.Shared.DTOs.Auth;

public class SignInResponseDto
{
    [Description("Access token to login the account")]
    public string AccessToken { get; set; }

    [Description("Refresh token to get another new access token")]
    public string RefreshToken { get; set; }
}