using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Event;

public class ReasonDto
{
    [SwaggerSchema("Unique identifier for the reason")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the reason")]
    public string Name { get; set; } = string.Empty;
}
