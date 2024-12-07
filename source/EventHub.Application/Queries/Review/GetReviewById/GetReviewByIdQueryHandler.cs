using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Review.GetReviewById;

public class GetReviewByIdQueryHandler : IQueryHandler<GetReviewByIdQuery, ReviewDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetReviewByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewDto> Handle(GetReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.ReviewAggregate.Review cachedReview = await _unitOfWork.CachedReviews.GetByIdAsync(request.ReviewId);

        if (cachedReview == null)
        {
            throw new NotFoundException("Review does not exist!");
        }

        ReviewDto review = _mapper.Map<ReviewDto>(cachedReview);

        return review;
    }
}
