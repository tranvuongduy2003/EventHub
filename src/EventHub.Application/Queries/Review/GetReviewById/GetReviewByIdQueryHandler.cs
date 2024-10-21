using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;

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


        var cachedReview = await _unitOfWork.CachedCategories.GetByIdAsync(request.ReviewId);

        if (cachedReview == null)
            throw new NotFoundException("Review does not exist!");

        var review = _mapper.Map<ReviewDto>(cachedReview);

        return review;
    }
}