using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Command;

public class CommandDto
{
    [SwaggerSchema("Unique identifier for the command.")]
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]  // Default value for Id
    public string Id { get; set; }

    [SwaggerSchema("The name of the command.")]
    [DefaultValue("Unnamed Command")]  // Default value for Name
    public string Name { get; set; }
}