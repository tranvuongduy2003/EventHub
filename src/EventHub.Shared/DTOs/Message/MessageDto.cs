using EventHub.Shared.DTOs.Conversation;

namespace EventHub.Shared.DTOs.Message;

public class MessageDto
{
    public string Id { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    public string? AudioUrl { get; set; } = string.Empty;

    public string UserId { get; set; }

    public ConversationUserDto User { get; set; }

    public string ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}