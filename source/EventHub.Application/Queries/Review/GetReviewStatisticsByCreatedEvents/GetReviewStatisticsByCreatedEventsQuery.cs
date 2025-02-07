using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Review.GetReviewStatisticsByCreatedEvents;

public record GetReviewStatisticsByCreatedEventsQuery(Guid AuthorId) : IQuery<ReviewStatisticsDto>;
