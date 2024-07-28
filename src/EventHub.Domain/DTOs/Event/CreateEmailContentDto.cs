using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.DTOs.Event;

public class CreateEmailContentDto
{
    public string Content { get; set; }

    public List<IFormFile>? Attachments { get; set; } = new();
}