using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.Event;

public class CreateEmailContentDto
{
    public string Content { get; set; }

    public List<IFormFile>? Attachments { get; set; } = new();
}