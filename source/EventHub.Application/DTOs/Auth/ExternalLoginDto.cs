using Microsoft.AspNetCore.Authentication;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Auth;

public class ExternalLoginDto
{
    [SwaggerSchema("Authentication properties for the external login")]
    public AuthenticationProperties Properties { get; set; } = new AuthenticationProperties();

    [SwaggerSchema("Name of the external authentication provider (e.g., Google, Facebook)")]
    public string Provider { get; set; } = "Google";
}
