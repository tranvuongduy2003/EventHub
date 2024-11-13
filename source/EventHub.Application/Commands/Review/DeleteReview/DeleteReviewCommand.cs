using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Review.DeleteReview;

/// <summary>
/// Represents a command to delete a review.
/// </summary>
/// <remarks>
/// This command is used to remove a review by its unique identifier.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the review to be deleted.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the review.
/// </param>
public record DeleteReviewCommand(Guid Id) : ICommand;