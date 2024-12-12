using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;

public class
    GetPaginatedReviewsByEventIdQueryHandler : IQueryHandler<GetPaginatedReviewsByEventIdQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        List<Domain.Aggregates.ReviewAggregate.Review> reviews = await _unitOfWork.CachedReviews
            .FindByCondition(x => x.EventId == request.EventId)
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync(cancellationToken);

        List<ReviewDto> reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}
