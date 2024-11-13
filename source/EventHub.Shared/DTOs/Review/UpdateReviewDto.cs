using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Review;

public class UpdateReviewDto
{
    [SwaggerSchema("Content of the review")]
    [DefaultValue("This event was amazing! The organization was top-notch and I had a great time.")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    [DefaultValue(5.0)]
    public double Rate { get; set; }
}