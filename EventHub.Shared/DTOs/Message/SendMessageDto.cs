namespace EventHub.Shared.DTOs.Message;

public class SendMessageDto
{
    public string UserId { get; set; }

    public string ConversationId { get; set; }

    public string? Content { get; set; }

    public string? ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoId { get; set; }

    public string? VideoUrl { get; set; }

    public string? AudioId { get; set; }

    public string? AudioUrl { get; set; }
}