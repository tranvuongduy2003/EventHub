using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class UpdateEmailContentDto
{
    [SwaggerSchema("Unique identifier for the email content")]
    [DefaultValue("d2f1eae2-5d9d-4b6e-a1cd-4f8f52877421")]
    public Guid Id { get; set; }

    [SwaggerSchema("Main content of the email")]
    [DefaultValue("Dear customer, thank you for attending our event. We hope to see you again soon!")]
    public string Content { get; set; } = string.Empty;

    [SwaggerSchema("Optional attachments to be included in the email")]
    public IFormFileCollection? Attachments { get; set; } = null;
}