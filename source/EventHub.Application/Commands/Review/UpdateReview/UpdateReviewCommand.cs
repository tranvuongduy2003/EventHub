using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Review.UpdateReview;

/// <summary>
/// Represents a command to update an existing review.
/// </summary>
/// <remarks>
/// This command is used to modify the details of a review by its unique identifier.
/// </remarks>
public class UpdateReviewCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateReviewCommand"/> class.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> representing the unique identifier of the review to be updated.
    /// </param>
    /// <param name="request">
    /// The data transfer object containing the updated review details.
    /// </param>
    public UpdateReviewCommand(Guid id, UpdateReviewDto request)
        => (Id, Content, Rate) = (id, request.Content, request.Rate);

    /// <summary>
    /// Gets or sets the unique identifier of the review to be updated.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the review.
    /// </value>
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public double Rate { get; set; }
}
