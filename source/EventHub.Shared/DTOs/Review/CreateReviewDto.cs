using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Review;

public class CreateReviewDto
{
    [SwaggerSchema("Unique identifier for the review author")]
    [DefaultValue("d5e8f9c3-a4b2-4e5f-9b3c-d1f6f5a6c8d9")]
    public Guid AuthorId { get; set; }

    [SwaggerSchema("Unique identifier for the event being reviewed")]
    [DefaultValue("e7f9c4d2-b5a3-4e6f-8c7d-f2f7e5c9a1b2")]
    public Guid EventId { get; set; }

    [SwaggerSchema("Content of the review")]
    [DefaultValue("This event was amazing! The organization was top-notch and I had a great time.")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    [DefaultValue(5.0)]
    public double Rate { get; set; }
}