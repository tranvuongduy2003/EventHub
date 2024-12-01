using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class ResetUserPasswordDto
{
    [SwaggerSchema("New password setted for the account")]
    public string NewPassword { get; set; }

    [SwaggerSchema("Email registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("Reset password token generated from server")]
    public string Token { get; set; }
}
