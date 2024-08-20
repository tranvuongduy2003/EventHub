namespace EventHub.Shared.DTOs.Conversation;

public class ConversationDto
{
    public string Id { get; set; }

    public string EventId { get; set; }

    public ConversationEventDto Event { get; set; }

    public string HostId { get; set; }

    public ConversationUserDto Host { get; set; }

    public string UserId { get; set; }

    public ConversationUserDto User { get; set; }

    public ConversationLastMessageDto? LastMessage { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}