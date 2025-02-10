namespace EventHub.Application.SeedWork.DTOs.Message;

public class EditMessageDto
{
    public Guid MessageId { get; set; }

    public Guid SenderId { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageFileName { get; set; }

    public string? VideoUrl { get; set; }

    public string? VideoFileName { get; set; }

    public string? AudioUrl { get; set; }

    public string? AudioFileName { get; set; }
}
