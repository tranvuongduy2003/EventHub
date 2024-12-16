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

    public Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.ReviewAggregate.Review> paginatedReviews = _unitOfWork.CachedReviews
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Author)
            );

        Pagination<ReviewDto> paginatedReviewDtos = _mapper.Map<Pagination<ReviewDto>>(paginatedReviews);

        return Task.FromResult(paginatedReviewDtos);
    }
}
