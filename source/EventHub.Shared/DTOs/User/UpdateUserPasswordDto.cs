using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class UpdateUserPasswordDto
{
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]
    [SwaggerSchema("Unique identifier for the user")]
    public Guid UserId { get; set; }

    [DefaultValue("OldPassword123!")]
    [SwaggerSchema("The current password of the user, required for verification")]
    public string OldPassword { get; set; }

    [DefaultValue("NewPassword123!")]
    [SwaggerSchema("The new password that the user wants to set")]
    public string NewPassword { get; set; }
}