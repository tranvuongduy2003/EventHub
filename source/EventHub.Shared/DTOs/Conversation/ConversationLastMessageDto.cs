using EventHub.Shared.DTOs.User;

namespace EventHub.Shared.DTOs.Conversation;

public class ConversationLastMessageDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public AuthorDto Sender { get; set; }
}