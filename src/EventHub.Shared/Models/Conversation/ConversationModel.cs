namespace EventHub.Shared.Models.Conversation;

public class ConversationModel
{
    public string Id { get; set; }

    public string EventId { get; set; }

    public ConversationEventModel Event { get; set; }

    public string HostId { get; set; }

    public ConversationUserModel Host { get; set; }

    public string UserId { get; set; }

    public ConversationUserModel User { get; set; }

    public ConversationLastMessageModel? LastMessage { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}