using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class UpdateUserPasswordDto
{
    [SwaggerSchema("Unique identifier for the user")]
    public Guid UserId { get; set; }

    [SwaggerSchema("The current password of the user, required for verification")]
    public string OldPassword { get; set; }

    [SwaggerSchema("The new password that the user wants to set")]
    public string NewPassword { get; set; }
}
