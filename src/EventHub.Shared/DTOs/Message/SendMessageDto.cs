namespace EventHub.Shared.DTOs.Message;

public class SendMessageDto
{
    public string UserId { get; set; }

    public string ConversationId { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }
    
    public string? ImageFileName { get; set; }

    public string? VideoUrl { get; set; }
    
    public string? VideoFileName { get; set; }

    public string? AudioUrl { get; set; }
    
    public string? AudioFileName { get; set; }
}
