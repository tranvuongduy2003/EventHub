using System.ComponentModel;

namespace EventHub.Shared.DTOs.Auth;

public class SignInDto
{
    [DefaultValue("admin@gmail.com")]
    [Description("Phone number or email registered for the account")]
    public string Identity { get; set; }

    [DefaultValue("Admin@123")]
    [Description("Password to login the account")]
    public string Password { get; set; }
}