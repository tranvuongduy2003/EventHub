using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Event;

public class UpdateEmailContentDto
{
    [SwaggerSchema("Unique identifier for the email content")]
    public Guid Id { get; set; }

    [SwaggerSchema("Main content of the email")]
    public string Content { get; set; } = string.Empty;

    [SwaggerSchema("Optional attachments to be included in the email")]
    public IFormFileCollection? Attachments { get; set; } = null;
}
