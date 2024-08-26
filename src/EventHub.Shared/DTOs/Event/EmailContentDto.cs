namespace EventHub.Shared.DTOs.Event;

public class EmailContentDto
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public List<string> AttachmentUrls { get; set; }
}