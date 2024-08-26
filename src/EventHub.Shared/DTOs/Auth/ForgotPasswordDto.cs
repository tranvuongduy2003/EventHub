using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Auth;

public class ForgotPasswordDto
{
    [DefaultValue("user123@gmail.com")]
    [SwaggerSchema("Email registerd for the account")]
    public required string Email { get; init; }
}