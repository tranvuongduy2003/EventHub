namespace EventHub.Shared.DTOs.Conversation;

public class ConversationDto
{
    public Guid Id { get; set; }

    public ConversationEventDto Event { get; set; }

    public ConversationUserDto Host { get; set; }

    public ConversationUserDto User { get; set; }

    public ConversationLastMessageDto? LastMessage { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}