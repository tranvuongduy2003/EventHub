using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;

public class
    GetPaginatedReviewsByEventIdQueryHandler : IQueryHandler<GetPaginatedReviewsByEventIdQuery, Pagination<ReviewDto>>
{
    private readonly ILogger<GetPaginatedReviewsByEventIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsByEventIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPaginatedReviewsByEventIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedReviewsByEventIdQueryHandler");

        var isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id.Equals(request.EventId));
        if (!isEventExisted)
            throw new NotFoundException("Event does not exist!");

        var reviews = await _unitOfWork.CachedReviews
            .FindByCondition(x => x.EventId.Equals(request.EventId))
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync();

        var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

        _logger.LogInformation("END: GetPaginatedReviewsByEventIdQueryHandler");

        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}