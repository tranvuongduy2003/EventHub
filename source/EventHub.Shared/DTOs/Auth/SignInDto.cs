using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Auth;

public class SignInDto
{
    [SwaggerSchema("Phone number or email registered for the account")]
    public string Identity { get; set; }

    [SwaggerSchema("Password to login the account")]
    public string Password { get; set; }
}
