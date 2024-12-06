namespace EventHub.Application.DTOs.Message;

public class SendMessageDto
{
    public Guid AuthorId { get; set; }

    public Guid ConversationId { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageFileName { get; set; }

    public string? VideoUrl { get; set; }

    public string? VideoFileName { get; set; }

    public string? AudioUrl { get; set; }

    public string? AudioFileName { get; set; }
}
