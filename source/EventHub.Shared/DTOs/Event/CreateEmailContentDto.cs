using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class CreateEmailContentDto
{
    [SwaggerSchema("Main content of the email")]
    [DefaultValue("Dear customer, thank you for attending our event. We hope to see you again soon!")]
    public string Content { get; set; }

    [SwaggerSchema("Optional attachments to be included in the email")]
    public IFormFileCollection? Attachments { get; set; } = null;
}