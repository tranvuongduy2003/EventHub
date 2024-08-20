using System.ComponentModel;

namespace EventHub.Shared.DTOs.Auth;

public class RefreshTokenDto
{
    [Description("Refresh token received after login")]
    public string RefreshToken { get; set; }
}