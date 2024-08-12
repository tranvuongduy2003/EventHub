using Microsoft.AspNetCore.Authentication;

namespace EventHub.Shared.Models.Auth;

public class ExternalLoginModel
{
    public AuthenticationProperties Properties { get; set; }

    public string Provider { get; set; }
}