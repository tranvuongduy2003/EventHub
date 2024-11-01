﻿using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
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
        var isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id.Equals(request.EventId));
        if (!isEventExisted)
            throw new NotFoundException("Event does not exist!");

        var reviews = await _unitOfWork.CachedReviews
            .FindByCondition(x => x.EventId.Equals(request.EventId))
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync();

        var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);


        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}