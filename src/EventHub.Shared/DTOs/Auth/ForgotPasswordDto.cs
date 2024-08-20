using System.ComponentModel;

namespace EventHub.Shared.DTOs.Auth;

public class ForgotPasswordDto
{
    [DefaultValue("user123@gmail.com")]
    [Description("Email registerd for the account")]
    public required string Email { get; init; }
}