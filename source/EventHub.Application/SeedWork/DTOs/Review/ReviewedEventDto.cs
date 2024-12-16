using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Review;

public class ReviewedEventDto
{
    [SwaggerSchema("Unique identifier of the event")]
    public Guid Id { get; set; }

    [SwaggerSchema("URL of the cover image for the event")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [SwaggerSchema("Name of the event")] 
    public string Name { get; set; } = string.Empty;
}
