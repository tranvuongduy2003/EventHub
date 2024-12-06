using EventHub.Application.DTOs.User;

namespace EventHub.Application.DTOs.Conversation;

public class ConversationLastMessageDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public AuthorDto Sender { get; set; }
}
