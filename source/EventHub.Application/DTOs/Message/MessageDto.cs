using EventHub.Application.DTOs.User;

namespace EventHub.Application.DTOs.Message;

public class MessageDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    public string? AudioUrl { get; set; } = string.Empty;

    public AuthorDto? Author { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
