using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.DTOs.User;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Review;

public class ReviewDto
{
    [SwaggerSchema("Unique identifier for the review")]
    public string Id { get; set; } = string.Empty;

    [SwaggerSchema("Content of the review")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    public double Rate { get; set; }

    [SwaggerSchema("Positive level analysed by the sentiment, true or false")]
    public bool IsPositive { get; set; }

    [SwaggerSchema("Event details associated with the review")]
    public LeanEventDto? Event { get; set; } = null;

    [SwaggerSchema("Author details associated with the review")]
    public AuthorDto? Author { get; set; } = null;

    [SwaggerSchema("Timestamp when the review was created")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("Timestamp when the review was last updated, if applicable")]
    public DateTime? UpdatedAt { get; set; }
}
