using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Auth;

public class SignUpDto
{
    [DefaultValue("user123@gmail.com")]
    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [DefaultValue("+84123456789")]
    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [DefaultValue("User 123")]
    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }

    [DefaultValue("User@123")]
    [SwaggerSchema("Password will be registered for the account")]
    public string Password { get; set; }

    [DefaultValue("user123")]
    [SwaggerSchema("User name will be registered for the account")]
    public string? UserName { get; set; } = string.Empty;
}