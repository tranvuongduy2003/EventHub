namespace EventHub.Domain.Models.Event;

public class EmailContentModel
{
    public string Id { get; set; }

    public string EventId { get; set; }

    public string Content { get; set; }

    public List<string> Attachments { get; set; }
}