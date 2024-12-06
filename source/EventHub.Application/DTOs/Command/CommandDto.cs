using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Command;

public class CommandDto
{
    [SwaggerSchema("Unique identifier for the command.")]
    public string Id { get; set; }

    [SwaggerSchema("The name of the command.")]
    public string Name { get; set; }
}
