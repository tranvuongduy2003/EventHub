namespace EventHub.Shared.Models.Review;

public class ReviewModel
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string UserAvatar { get; set; }

    public string EventId { get; set; }

    public string EventName { get; set; }

    public string EventCoverImage { get; set; }

    public string? Content { get; set; }

    public double Rate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}