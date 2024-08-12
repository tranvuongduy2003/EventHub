using EventHub.Shared.Models.Conversation;

namespace EventHub.Shared.Models.Message;

public class MessageModel
{
    public string Id { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public string? Video { get; set; }

    public string? Audio { get; set; }

    public string UserId { get; set; }

    public ConversationUserModel User { get; set; }

    public string ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}