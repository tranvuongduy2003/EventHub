using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Auth;

public class ForgotPasswordDto
{
    [SwaggerSchema("Email registerd for the account")]
    public required string Email { get; init; }
}
