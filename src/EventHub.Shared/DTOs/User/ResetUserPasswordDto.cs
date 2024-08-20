using System.ComponentModel;

namespace EventHub.Shared.DTOs.User;

public class ResetUserPasswordDto
{
    [DefaultValue("User@123")]
    [Description("New password setted for the account")]
    public string NewPassword { get; set; }

    [DefaultValue("user123@gmail.com")]
    [Description("Email registered for the account")]
    public string Email { get; set; }

    [Description("Reset password token generated from server")]
    public string Token { get; set; }
}