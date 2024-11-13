using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Review;

namespace EventHub.Application.Commands.Review.CreateReview;

/// <summary>
/// Represents a command to create a review for an event.
/// </summary>
/// <remarks>
/// This command is used to create a new review with the specified details for a particular event.
/// </remarks>
public class CreateReviewCommand : ICommand<ReviewDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateReviewCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new review.
    /// </param>
    public CreateReviewCommand(CreateReviewDto request)
        => (AuthorId, EventId, Content, Rate) = (request.AuthorId, request.EventId, request.Content, request.Rate);

    /// <summary>
    /// Gets or sets the unique identifier of the author of the review.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the user who wrote the review.
    /// </value>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the event being reviewed.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the event.
    /// </value>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the content of the review.
    /// </summary>
    /// <value>
    /// A string representing the content of the review. Can be null.
    /// </value>
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the rating given to the event in the review.
    /// </summary>
    /// <value>
    /// A double representing the rating for the event, typically on a scale of 1 to 5.
    /// </value>
    public double Rate { get; set; }
}