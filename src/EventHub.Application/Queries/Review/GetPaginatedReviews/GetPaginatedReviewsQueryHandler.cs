using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Review.GetPaginatedReviews;

public class GetPaginatedReviewsQueryHandler : IQueryHandler<GetPaginatedReviewsQuery, Pagination<ReviewDto>>
{
    private readonly ILogger<GetPaginatedReviewsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPaginatedReviewsQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedReviewsQueryHandler");

        var reviews = await _unitOfWork.CachedReviews.FindAll()
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync();

        var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

        _logger.LogInformation("END: GetPaginatedReviewsQueryHandler");

        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}