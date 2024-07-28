namespace EventHub.Domain.DTOs.Conversation;

public class JoinChatRoomDto
{
    public string EventId { get; set; }

    public string HostId { get; set; }

    public string UserId { get; set; }
}