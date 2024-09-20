using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Auth;

public class ExternalLoginDto
{
    [SwaggerSchema("Authentication properties for the external login")]
    public AuthenticationProperties Properties { get; set; } = new AuthenticationProperties();

    [SwaggerSchema("Name of the external authentication provider (e.g., Google, Facebook)")]
    [DefaultValue("Google")]
    public string Provider { get; set; } = "Google";
}