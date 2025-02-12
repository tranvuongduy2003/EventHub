using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetReviewStatisticsByCreatedEvents;

public class GetReviewStatisticsByCreatedEventsQueryHandler : IQueryHandler<GetReviewStatisticsByCreatedEventsQuery, ReviewStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReviewStatisticsByCreatedEventsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<ReviewStatisticsDto> Handle(GetReviewStatisticsByCreatedEventsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = _unitOfWork.Reviews
            .FindAll()
            .Include(x => x.Event)
            .Where(x => x.Event.AuthorId == request.AuthorId)
            .ToList();

        int total = reviews.Count;
        int totalPositive = reviews.Count(x => x.IsPositive == true);
        int totalNegative = reviews.Count(x => x.IsPositive == false);
        double averageRate = reviews.Any() ? reviews.Average(x => x.Rate) : 0.0;

        var totalPerRate = Enumerable.Range(1, 5)
            .Select(rate => new RateStatistic
            {
                Rate = rate,
                Value = reviews.Count(x => x.Rate >= rate)
            })
            .ToList();

        return Task.FromResult(new ReviewStatisticsDto
        {
            Total = total,
            TotalPositive = totalPositive,
            TotalNegative = totalNegative,
            AverageRate = averageRate,
            TotalPerRate = totalPerRate
        });
    }
}
