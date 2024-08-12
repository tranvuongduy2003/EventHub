namespace EventHub.Shared.DTOs.Review;

public class CreateReviewDto
{
    public string UserId { get; set; }

    public string EventId { get; set; }

    public string? Content { get; set; }

    public double Rate { get; set; }
}