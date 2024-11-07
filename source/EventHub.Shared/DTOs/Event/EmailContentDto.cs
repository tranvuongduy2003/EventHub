using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class EmailContentDto
{
    [SwaggerSchema("Unique identifier for the email content")]
    [DefaultValue("f6a5d4e3-1b2c-3d4e-5f6g-7h8i9j0k1l2m")]
    public Guid Id { get; set; }

    [SwaggerSchema("Main content of the email message")]
    [DefaultValue("Thank you for attending our event. We hope you had a great experience!")]
    public string Content { get; set; } = string.Empty;

    [SwaggerSchema("List of URLs pointing to the attachments for the email")]
    [DefaultValue(new[] { "https://example.com/attachment1.pdf", "https://example.com/attachment2.jpg" })]
    public List<string> AttachmentUrls { get; set; } = new();
}