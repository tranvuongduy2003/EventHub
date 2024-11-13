using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class ResetUserPasswordDto
{
    [DefaultValue("User@123")]
    [SwaggerSchema("New password setted for the account")]
    public string NewPassword { get; set; }

    [DefaultValue("user123@gmail.com")]
    [SwaggerSchema("Email registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("Reset password token generated from server")]
    public string Token { get; set; }
}