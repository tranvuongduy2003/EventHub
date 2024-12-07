using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Review.GetReviewById;

/// <summary>
/// Represents a query to retrieve a review's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a review's information based on its unique identifier.
/// </remarks>
public record GetReviewByIdQuery(Guid ReviewId) : IQuery<ReviewDto>;