using EventHub.Application.SeedWork.DTOs.User;

namespace EventHub.Application.SeedWork.DTOs.Conversation;

public class ConversationLastMessageDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public AuthorDto? Sender { get; set; }

    public AuthorDto? Receiver { get; set; }
}
