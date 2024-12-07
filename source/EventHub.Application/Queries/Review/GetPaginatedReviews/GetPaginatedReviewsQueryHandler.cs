using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetPaginatedReviews;

public class GetPaginatedReviewsQueryHandler : IQueryHandler<GetPaginatedReviewsQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.Aggregates.ReviewAggregate.Review> reviews = await _unitOfWork.CachedReviews
            .FindAll()
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync(cancellationToken);

        List<ReviewDto> reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}
