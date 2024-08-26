namespace EventHub.Shared.DTOs.Event;

public class CreateFavouriteEventDto
{
    public Guid EventId { get; set; }

    public Guid UserId { get; set; }
}