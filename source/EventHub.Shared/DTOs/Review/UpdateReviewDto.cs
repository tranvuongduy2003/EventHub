using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Review;

public class UpdateReviewDto
{
    [SwaggerSchema("Content of the review")]
    public string? Content { get; set; }

    [SwaggerSchema("Rating provided by the reviewer, from 0 to 5")]
    public double Rate { get; set; }
}
