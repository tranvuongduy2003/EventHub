using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class CreateFavouriteEventDto
{
    [SwaggerSchema("Unique identifier of the event to be marked as favourite")]
    public Guid EventId { get; set; }

    [SwaggerSchema("Unique identifier of the user who is marking the event as favourite")]
    public Guid UserId { get; set; }
}
