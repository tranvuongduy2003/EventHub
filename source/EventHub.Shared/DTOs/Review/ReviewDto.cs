using System.ComponentModel;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.DTOs.User;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Review;

public class ReviewDto
{
    [SwaggerSchema("Unique identifier for the review")]
    [DefaultValue("9f8d7e6a-b5c3-4d2e-9a7b-f2e7c6d5b9f8")]
    public string Id { get; set; } = string.Empty;

    [SwaggerSchema("Content of the review")]
    [DefaultValue("Great event, had a wonderful time!")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    [DefaultValue(4.5)]
    public double Rate { get; set; }

    [SwaggerSchema("Event details associated with the review")]
    public EventDto? Event { get; set; } = null;

    [SwaggerSchema("Author details associated with the review")]
    public AuthorDto? Author { get; set; } = null;

    [SwaggerSchema("Timestamp when the review was created")]
    [DefaultValue("2024-09-21T12:00:00Z")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("Timestamp when the review was last updated, if applicable")]
    [DefaultValue(null)]
    public DateTime? UpdatedAt { get; set; }
}