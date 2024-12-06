using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Event;

public class EmailContentDto
{
    [SwaggerSchema("Unique identifier for the email content")]
    public Guid Id { get; set; }

    [SwaggerSchema("Main content of the email message")]
    public string Content { get; set; } = string.Empty;

    [SwaggerSchema("List of URLs pointing to the attachments for the email")]
    public List<string> AttachmentUrls { get; set; } = new();
}
