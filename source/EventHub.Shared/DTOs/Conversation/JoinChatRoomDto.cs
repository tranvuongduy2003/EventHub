namespace EventHub.Shared.DTOs.Conversation;

public class JoinChatRoomDto
{
    public Guid EventId { get; set; }

    public Guid HostId { get; set; }

    public Guid UserId { get; set; }
}
