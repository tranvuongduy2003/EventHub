using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class CreateFavouriteEventDto
{
    [SwaggerSchema("Unique identifier of the event to be marked as favourite")]
    [DefaultValue("a1b2c3d4-e5f6-7890-ab12-cd34ef56gh78")]
    public Guid EventId { get; set; }

    [SwaggerSchema("Unique identifier of the user who is marking the event as favourite")]
    [DefaultValue("d9e8f7g6-h5i4-321j-klmn-op12qr34st56")]
    public Guid UserId { get; set; }
}