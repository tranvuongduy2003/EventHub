using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Auth;

public class SignUpDto
{
    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }

    [SwaggerSchema("Password will be registered for the account")]
    public string Password { get; set; }

    [SwaggerSchema("User name will be registered for the account")]
    public string? UserName { get; set; } = string.Empty;
}
