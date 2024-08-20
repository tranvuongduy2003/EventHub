using Microsoft.AspNetCore.Authentication;

namespace EventHub.Shared.DTOs.Auth;

public class ExternalLoginDto
{
    public AuthenticationProperties Properties { get; set; }

    public string Provider { get; set; }
}