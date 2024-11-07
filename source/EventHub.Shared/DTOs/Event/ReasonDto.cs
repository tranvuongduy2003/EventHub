using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class ReasonDto
{
    [SwaggerSchema("Unique identifier for the reason")]
    [DefaultValue("f8e2b9c3-a1d2-4e5f-9b3c-d1e6f5e7a8b9")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the reason")]
    [DefaultValue("Networking Opportunity")]
    public string Name { get; set; } = string.Empty;
}