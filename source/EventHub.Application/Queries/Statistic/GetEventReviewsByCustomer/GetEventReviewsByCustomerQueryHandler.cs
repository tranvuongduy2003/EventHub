using EventHub.Application.SeedWork.DTOs.Statistic;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetEventReviewsByCustomer;

public class GetEventReviewsByCustomerQueryHandler : IQueryHandler<GetEventReviewsByCustomerQuery, EventReviewsByCustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventReviewsByCustomerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<EventReviewsByCustomerDto> Handle(GetEventReviewsByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = _unitOfWork.Reviews
                    .FindByCondition(x => x.EventId == request.EventId)
                    .ToList();

        int total = reviews.Count;
        int totalPositive = reviews.Count(x => x.IsPositive == true);
        int totalNegative = reviews.Count(x => x.IsPositive == false);
        double averageRate = reviews.Any() ? reviews.Average(x => x.Rate) : 0.0;

        return Task.FromResult(new EventReviewsByCustomerDto
        {
            TotalReviews = total,
            TotalNegative = totalNegative,
            TotalPositive = totalPositive,
            AvarageRate = averageRate
        });
    }
}
