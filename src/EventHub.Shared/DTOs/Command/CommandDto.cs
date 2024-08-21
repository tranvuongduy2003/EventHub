using System.ComponentModel;

namespace EventHub.Shared.DTOs.Command;

public class CommandDto
{
    [Description("Unique identifier for the command.")]
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]  // Default value for Id
    public string Id { get; set; }

    [Description("The name of the command.")]
    [DefaultValue("Unnamed Command")]  // Default value for Name
    public string Name { get; set; }
}