namespace EventHub.Application.SeedWork.DTOs.Message;

public class DeleteMessageDto
{
    public Guid MessageId { get; set; }

    public Guid SenderId { get; set; }
}
