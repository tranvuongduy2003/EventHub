using EventHub.Shared.DTOs.Conversation;

namespace EventHub.Shared.DTOs.Message;

public class MessageDto
{
    public string Id { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public string? Video { get; set; }

    public string? Audio { get; set; }

    public string UserId { get; set; }

    public ConversationUserDto User { get; set; }

    public string ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}