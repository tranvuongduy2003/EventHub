using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Event;

public class CreateEmailContentDto
{
    [SwaggerSchema("Main content of the email")]
    public string Content { get; set; }

    [SwaggerSchema("Optional attachments to be included in the email")]
    public IFormFileCollection? Attachments { get; set; } = null;
}
