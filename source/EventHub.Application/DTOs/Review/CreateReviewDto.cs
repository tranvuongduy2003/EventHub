using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Review;

public class CreateReviewDto
{
    [SwaggerSchema("Unique identifier for the review author")]
    public Guid AuthorId { get; set; }

    [SwaggerSchema("Unique identifier for the event being reviewed")]
    public Guid EventId { get; set; }

    [SwaggerSchema("Content of the review")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    public double Rate { get; set; }
}
